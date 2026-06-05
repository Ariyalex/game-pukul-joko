using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// GameplayScene HUD + Game Over UI. Displays score, countdown timer, stage label and the
/// 3-strike indicator. Wires Retry (reload gameplay) and Menu (load menu) buttons.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Strike Indicator")]
[Tooltip("One image per strike slot (left to right). Filled when used.")]
    [SerializeField] private Image[] strikeIcons;
    [SerializeField] private Color strikeEmptyColor = new Color(1f, 1f, 1f, 0.25f);
    [SerializeField] private Color strikeFilledColor = Color.red;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverTitleText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    [Header("Scene names")]
    [SerializeField] private string gameplaySceneName = "GameplayScene";
    [SerializeField] private string menuSceneName = "MenuScene";

    private void Start()
    {
        if (retryButton != null)
            retryButton.onClick.AddListener(() => SceneManager.LoadScene(gameplaySceneName));
        if (menuButton != null)
            menuButton.onClick.AddListener(() => SceneManager.LoadScene(menuSceneName));

        HideGameOver();
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void UpdateTimer(float timeRemaining)
    {
        if (timerText != null) timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    public void SetStage(int stage)
    {
        if (stageText != null) stageText.text = "Stage " + stage;
    }

    public void UpdateCountdown(string text)
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = text;
        }
    }

    public void HideCountdown()
    {
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    /// <summary>Updates strike icons; first <paramref name="strikes"/> slots become filled.</summary>
public void SetStrikes(int strikes)
    {
        if (strikeIcons == null) return;
        for (int i = 0; i < strikeIcons.Length; i++)
        {
            if (strikeIcons[i] == null) continue;
            strikeIcons[i].color = i < strikes ? strikeFilledColor : strikeEmptyColor;
        }
    }

    public void ShowGameOver(bool won, int finalScore)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (gameOverTitleText != null) gameOverTitleText.text = won ? "TIME UP!" : "GAME OVER";
        if (finalScoreText != null) finalScoreText.text = "Final Score: " + finalScore;
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }
}
