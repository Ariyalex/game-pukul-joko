using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// MenuScene controller. Lets the player pick a difficulty (Normal/Hard) and a stage (1-3),
/// stores the choices in the persistent <see cref="GameSession"/>, then loads the gameplay scene.
/// </summary>
public class MenuUIManager : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Color selectedTint = Color.white;
    [SerializeField] private Color unselectedTint = new Color(0.7f, 0.7f, 0.7f, 1f);

    [Header("Stage Selection")]
    [Tooltip("Stage buttons in order: index 0 = Stage 1, etc.")]
    [SerializeField] private Button[] stageButtons;

    [Header("Flow")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI selectionLabel;
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    [Header("Animation")]
    [SerializeField] private float selectedScale = 1.15f;
    [SerializeField] private float selectedYOffset = 15f;
    [SerializeField] private float animDuration = 0.2f;

    private Difficulty _difficulty = Difficulty.Normal;
    private int _stage = 1;
    private System.Collections.Generic.Dictionary<Button, float> _originalY = new System.Collections.Generic.Dictionary<Button, float>();
    private System.Collections.Generic.Dictionary<Button, Vector3> _initialScale = new System.Collections.Generic.Dictionary<Button, Vector3>();

    private void Start()
    {
        if (normalButton != null)
        {
            _originalY[normalButton] = normalButton.transform.localPosition.y;
            _initialScale[normalButton] = normalButton.transform.localScale;
            normalButton.onClick.AddListener(() => SetDifficulty(Difficulty.Normal));
        }
        if (hardButton != null)
        {
            _originalY[hardButton] = hardButton.transform.localPosition.y;
            _initialScale[hardButton] = hardButton.transform.localScale;
            hardButton.onClick.AddListener(() => SetDifficulty(Difficulty.Hard));
        }

        if (stageButtons != null)
            for (int i = 0; i < stageButtons.Length; i++)
            {
                int stage = i + 1; // capture
                if (stageButtons[i] != null)
                {
                    _originalY[stageButtons[i]] = stageButtons[i].transform.localPosition.y;
                    _initialScale[stageButtons[i]] = stageButtons[i].transform.localScale;
                    stageButtons[i].onClick.AddListener(() => SetStage(stage));
                }
            }

        if (playButton != null) playButton.onClick.AddListener(Play);
        if (quitButton != null) quitButton.onClick.AddListener(Quit);

        // Default selection.
        SetDifficulty(Difficulty.Normal);
        SetStage(1);

        AudioManager.Instance?.PlayBGM(BgmType.Menu);
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        _difficulty = difficulty;
        AnimateSelection(normalButton, difficulty == Difficulty.Normal);
        AnimateSelection(hardButton, difficulty == Difficulty.Hard);
        RefreshLabel();
    }

    private void SetStage(int stage)
    {
        _stage = Mathf.Clamp(stage, 1, 3);
        if (stageButtons != null)
            for (int i = 0; i < stageButtons.Length; i++)
                AnimateSelection(stageButtons[i], (i + 1) == _stage);
        RefreshLabel();
    }

    private void RefreshLabel()
    {
        if (selectionLabel != null)
            selectionLabel.text = $"{_difficulty} - Stage {_stage}";
    }

    private void Play()
    {
        if (GameSession.Instance != null)
        {
            GameSession.Instance.SetDifficulty(_difficulty);
            GameSession.Instance.SetStage(_stage);
        }
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void AnimateSelection(Button button, bool selected)
    {
        if (button == null) return;
        
        button.transform.DOKill();
        
        Vector3 initial = _initialScale.ContainsKey(button) ? _initialScale[button] : Vector3.one;
        Vector3 targetScale = selected ? initial * selectedScale : initial;
        float baseY = _originalY.ContainsKey(button) ? _originalY[button] : button.transform.localPosition.y;
        float targetY = selected ? baseY + selectedYOffset : baseY;

        button.transform.DOScale(targetScale, animDuration).SetEase(Ease.OutBack);
        button.transform.DOLocalMoveY(targetY, animDuration).SetEase(Ease.OutBack);
        
        var glow = button.transform.Find("GlowEffect");
        if (glow != null) glow.gameObject.SetActive(selected);
    }
}
