using UnityEngine;

/// <summary>
/// Data describing one target archetype (Normal / Penalty / Tough).
/// Drives visuals, hit count, scoring and penalty behaviour without code branching.
/// Greybox-friendly: holds a placeholder tint now, and sprite slots for later swapping.
/// </summary>
[CreateAssetMenu(fileName = "WhackableConfig", menuName = "Pukul Joko/Whackable Config")]
public class WhackableConfig : ScriptableObject
{
    [Header("Identity")]
    public WhackableType type = WhackableType.Normal;

    [Header("Gameplay")]
    [Tooltip("Taps required to defeat. Normal/Penalty = 1, Tough = 2.")]
    [Min(1)] public int hitsRequired = 1;

    [Tooltip("Score granted on defeat (ignored for penalty objects).")]
    public int scoreValue = 10;

    [Tooltip("If true, tapping this costs the player a strike instead of granting score.")]
    public bool isPenalty = false;

    [Tooltip("Score granted if this target retreats without being hit (e.g. reward for avoiding bombs).")]
    public int skipScoreBonus = 0;

    [Header("Visuals - Sprite Swap Ready")]
[Tooltip("Default appearance.")]
    public Sprite normalSprite;
    [Tooltip("Shown after a non-defeating hit (e.g. Tough first hit).")]
    public Sprite damagedSprite;
    [Tooltip("Shown as the 'hit' frame or defeated state.")]
    public Sprite hitSprite;

    [Header("Greybox Placeholder")]
    [Tooltip("Default tint applied to the primitive. Set to white once real art is assigned.")]
    public Color greyboxColor = Color.white;
    [Tooltip("Tint used for the hit/damaged/defeat state. Set to white once real art is assigned.")]
    public Color hitColor = Color.white;
}
