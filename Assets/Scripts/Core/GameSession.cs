using UnityEngine;

/// <summary>
/// Lightweight, persistent (DontDestroyOnLoad) data carrier that travels between
/// MenuScene and GameplayScene. Holds nothing but the player's pre-round choices.
/// No gameplay logic lives here.
/// </summary>
public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    /// <summary>Chosen difficulty (affects only the speed multiplier in GameManager/SpawnManager).</summary>
    public Difficulty SelectedDifficulty { get; private set; } = Difficulty.Normal;

    /// <summary>Chosen stage to play (1..3). Each stage is a discrete, self-contained round.</summary>
    public int SelectedStage { get; private set; } = 1;

    private void Awake()
    {
        // Enforce single persistent instance.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Called by the menu before loading the gameplay scene.</summary>
    public void SetDifficulty(Difficulty difficulty) => SelectedDifficulty = difficulty;

    /// <summary>Called by the menu before loading the gameplay scene.</summary>
    public void SetStage(int stage) => SelectedStage = Mathf.Clamp(stage, 1, 3);
}
