using UnityEngine;

/// <summary>
/// Orchestrates a single gameplay round (one discrete, selectable stage).
/// Reads the player's choices from <see cref="GameSession"/>, configures the
/// <see cref="SpawnManager"/> with the active stage + difficulty factor, runs the
/// 30s timer, tracks score, and manages the 3-strike penalty system.
///
/// Win  = timer reaches 0 with fewer than MaxStrikes strikes.
/// Lose = strikes reach MaxStrikes (immediate Game Over).
/// </summary>
public class GameManager : MonoBehaviour
{
    public const int MaxStrikes = 3;

    public static GameManager Instance { get; private set; }

    [Header("Scene references")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private UIManager uiManager;

    [Header("Config")]
    [Tooltip("Stage assets in order: index 0 = Stage 1, 1 = Stage 2, 2 = Stage 3.")]
    [SerializeField] private StageConfig[] stageConfigs;
    [SerializeField] private DifficultyConfig difficultyConfig;

    public GameState CurrentState { get; private set; } = GameState.Intro;

    private StageConfig _activeStage;
    private int _score;
    private int _strikes;
    private float _timeLeft;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Resolve the chosen stage/difficulty (fallback to Stage 1 / Normal in dev).
        int stageIndex = (GameSession.Instance != null ? GameSession.Instance.SelectedStage : 1) - 1;
        Difficulty difficulty = GameSession.Instance != null
            ? GameSession.Instance.SelectedDifficulty
            : Difficulty.Normal;

        stageIndex = Mathf.Clamp(stageIndex, 0, (stageConfigs?.Length ?? 1) - 1);
        _activeStage = (stageConfigs != null && stageConfigs.Length > 0) ? stageConfigs[stageIndex] : null;

        _score = 0;
        _strikes = 0;
        _timeLeft = _activeStage != null ? _activeStage.roundDuration : 30f;

        // Initialise UI.
        if (uiManager != null)
        {
            uiManager.SetStage(_activeStage != null ? _activeStage.stageIndex : stageIndex + 1);
            uiManager.UpdateScore(_score);
            uiManager.SetStrikes(_strikes);
            uiManager.UpdateTimer(_timeLeft);
            uiManager.HideGameOver();
        }

        // Stage content is enforced purely by allowedTypes (data-driven, no per-stage branching).
        if (spawnManager != null)
        {
            spawnManager.Configure(_activeStage, difficulty);
        }

        uiManager?.HideCountdown();
        StartCoroutine(CountdownRoutine());
    }

    private System.Collections.IEnumerator CountdownRoutine()
    {
        CurrentState = GameState.Intro;
        
        if (uiManager != null)
        {
            uiManager.UpdateCountdown("3");
            yield return new WaitForSeconds(1f);
            uiManager.UpdateCountdown("2");
            yield return new WaitForSeconds(1f);
            uiManager.UpdateCountdown("1");
            yield return new WaitForSeconds(1f);
            uiManager.UpdateCountdown("GO!");
            yield return new WaitForSeconds(0.5f);
            uiManager.HideCountdown();
        }

        AudioManager.Instance?.PlayBGM(BgmType.Gameplay);
        CurrentState = GameState.Playing;

        if (spawnManager != null)
        {
            spawnManager.StartSpawning();
        }
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing) return;

        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f)
        {
            _timeLeft = 0f;
            uiManager?.UpdateTimer(_timeLeft);
            EndGame(won: true);   // survived to time-up
            return;
        }
        uiManager?.UpdateTimer(_timeLeft);
    }

    /// <summary>Adds score (defeats only). Ignored outside play.</summary>
    public void AddScore(int amount)
    {
        if (CurrentState != GameState.Playing) return;
        _score += amount;
        uiManager?.UpdateScore(_score);
    }

    /// <summary>
    /// 3-strike penalty system. Called when the player taps a penalty (bomb) object.
    /// On the third strike the round ends immediately.
    /// </summary>
    public void RegisterPenalty()
    {
        if (CurrentState != GameState.Playing) return;

        _strikes++;
        AudioManager.Instance?.PlaySFX("Strike");
        uiManager?.SetStrikes(_strikes);

        if (_strikes >= MaxStrikes)
            EndGame(won: false);
    }

    /// <summary>Ends the round and shows the appropriate Game Over screen.</summary>
    public void EndGame(bool won)
    {
        if (CurrentState == GameState.GameOver) return;

        CurrentState = GameState.GameOver;
        spawnManager?.StopAll();
        uiManager?.ShowGameOver(won, _score);
    }
}
