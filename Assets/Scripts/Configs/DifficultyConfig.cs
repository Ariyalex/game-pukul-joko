using UnityEngine;

/// <summary>
/// Base gameplay timings plus the single multiplier that separates Normal from Hard.
/// Per design, Hard differs ONLY by multiplying spawn interval, pop duration and stay
/// duration by <see cref="hardMultiplier"/> (smaller = faster).
/// </summary>
[CreateAssetMenu(fileName = "DifficultyConfig", menuName = "Pukul Joko/Difficulty Config")]
public class DifficultyConfig : ScriptableObject
{
    [Header("Base Timings (Normal mode)")]
    [Tooltip("Seconds between spawns.")]
    public float baseSpawnInterval = 1.2f;

    [Tooltip("Seconds for the pop-up / hide DOTween move.")]
    public float basePopDuration = 0.25f;

    [Tooltip("Seconds a target stays up before auto-hiding if not defeated.")]
    public float baseStayDuration = 1.0f;

    [Header("Hard Mode")]
    [Tooltip("Multiplier applied to all timings in Hard mode (< 1 = faster). " +
             "0.6 makes Hard significantly faster.")]
    [Range(0.1f, 1f)] public float hardMultiplier = 0.6f;

    /// <summary>Returns the timing multiplier for the given difficulty (Normal = 1).</summary>
    public float GetFactor(Difficulty difficulty)
        => difficulty == Difficulty.Hard ? hardMultiplier : 1f;
}
