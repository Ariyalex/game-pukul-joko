using DG.Tweening;
using UnityEngine;

/// <summary>
/// A single spawnable target that pops out of a hole. Pooled: one per <see cref="Hole"/>.
/// Behaviour is fully data-driven by its <see cref="WhackableConfig"/> (Normal/Penalty/Tough).
/// All motion uses DOTween (no Update math). The owning Hole drives spawn/hide and is
/// notified back via callbacks when this object finishes hiding.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Whackable : MonoBehaviour
{
    [Header("FX")]
    [SerializeField] private ParticleSystem stars; // played on defeat

    private SpriteRenderer _sr;
    private BoxCollider2D _col;
    private Hole _hole;                 // owning hole (callback target)
    private WhackableConfig _config;
    private int _remainingHits;
    private bool _active;               // true while up and tappable
    private float _hiddenY;             // local Y when tucked inside the hole
    private float _shownY;              // local Y when popped up
    private float _popDuration = 0.25f;
    private float _stayDuration = 1.0f;
    private Tween _autoHideTween;       // delayed auto-hide handle

    public bool IsActive => _active;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<BoxCollider2D>();
        _sr.sortingLayerName = "Object";
        SetInteractable(false);
    }

    /// <summary>Called once by the owning Hole to wire references and rest positions.</summary>
    public void Initialise(Hole hole, float hiddenY, float shownY, int sortingOrder)
    {
        _hole = hole;
        _hiddenY = hiddenY;
        _shownY = shownY;
        
        if (_sr == null) _sr = GetComponent<SpriteRenderer>();
        _sr.sortingOrder = sortingOrder;
        
        var p = transform.localPosition;
        transform.localPosition = new Vector3(p.x, _hiddenY, p.z);
    }

    /// <summary>Configures appearance/stats and pops the target up. Schedules auto-hide.</summary>
    public void Spawn(WhackableConfig config, float popDuration, float stayDuration)
    {
        _config = config;
        _remainingHits = Mathf.Max(1, config.hitsRequired);
        _popDuration = popDuration;
        _stayDuration = stayDuration;

        ShowDefault();
KillTweens();

        // Pop up.
        transform.DOLocalMoveY(_shownY, popDuration).SetEase(Ease.OutBack);
        SetInteractable(true);

        // Auto-hide if the player ignores it.
        _autoHideTween = DOVirtual.DelayedCall(stayDuration, () =>
        {
            if (_active) Hide(defeated: false);
        });
    }

    /// <summary>Called by the InputManager when this collider is tapped.</summary>
    public void OnHit()
    {
        if (!_active) return;

        // Penalty: a single tap costs a strike, then it disappears.
        if (_config.isPenalty)
        {
            SetInteractable(false);
            AudioManager.Instance?.PlaySFX("Bomb");
            GameManager.Instance?.RegisterPenalty();
            transform.DOShakePosition(0.25f, 0.3f, 20, 90, false, true);
            ShowHit();
            Hide(defeated: true);
            return;
        }

        _remainingHits--;

        // Tough object surviving the first hit: feedback only, no score.
        if (_remainingHits > 0)
        {
            AudioManager.Instance?.PlaySFX("Hit");
            ShowDamaged();
            transform.DOShakePosition(0.2f, 0.25f, 18, 90, false, true);
            
            // Reset retreat timer to give player time for the second hit.
            _autoHideTween?.Kill();
            _autoHideTween = DOVirtual.DelayedCall(_stayDuration, () =>
            {
                if (_active) Hide(defeated: false);
            });
            return;
        }

        // Defeated.
        SetInteractable(false);
        AudioManager.Instance?.PlaySFX("Defeat");
        ShowHit();
        if (stars != null) stars.Play();
        transform.DOPunchScale(Vector3.one * 0.25f, 0.18f, 8, 0.6f);
        GameManager.Instance?.AddScore(_config.scoreValue);
        Hide(defeated: true);
    }

    /// <summary>Tucks the target back into the hole, then frees the hole.</summary>
    public void Hide(bool defeated)
    {
        SetInteractable(false);
        _autoHideTween?.Kill();
        _autoHideTween = null;

        // If skipped (not hit) and has a skip bonus (e.g. avoiding a penalty), grant points.
        if (!defeated && _config != null && _config.skipScoreBonus != 0)
        {
            GameManager.Instance?.AddScore(_config.skipScoreBonus);
        }

        // Slight delay so defeat punch/shake reads before retracting.
        float delay = defeated ? 0.1f : 0f;
transform.DOLocalMoveY(_hiddenY, _popDuration)
            .SetDelay(delay)
            .SetEase(Ease.InBack)
            .OnComplete(() => _hole?.OnWhackableHidden());
    }

    /// <summary>Cancels any in-flight tweens (call before re-spawn / on disable).</summary>
    public void KillTweens()
    {
        _autoHideTween?.Kill();
        _autoHideTween = null;
        transform.DOKill();
    }

    /// <summary>Default appearance. Swaps sprite if configured; greyboxColor tints (white once art exists).</summary>
    private void ShowDefault()
    {
        if (_config == null) return;
        if (_config.normalSprite != null) _sr.sprite = _config.normalSprite;
        _sr.color = _config.greyboxColor;
    }

    /// <summary>Damaged (non-defeating) appearance. Swaps to damagedSprite if configured.</summary>
    private void ShowDamaged()
    {
        if (_config == null) return;
        if (_config.damagedSprite != null) _sr.sprite = _config.damagedSprite;
        // Keep normal color for now or use a mid-tint
    }

    /// <summary>Hit/damaged/defeat appearance. Swaps to hitSprite if configured; hitColor tints.</summary>
    private void ShowHit()
    {
        if (_config == null) return;
        if (_config.hitSprite != null) _sr.sprite = _config.hitSprite;
        _sr.color = _config.hitColor;
    }

    private void SetInteractable(bool on)
    {
        _active = on;
        if (_col != null) _col.enabled = on;
    }

    private void OnDisable() => KillTweens();
}
