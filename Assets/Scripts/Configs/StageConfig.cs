using UnityEngine;

/// <summary>
/// Data describing a single stage (a discrete, selectable 30s round).
/// The spawnable types are restricted to <see cref="allowedTypes"/>, so stage content
/// is data-driven (no per-stage if/else in spawn code).
/// Stage 1: { Normal }  |  Stage 2: { Normal, Penalty }  |  Stage 3: { Normal, Penalty, Tough }.
/// </summary>
[CreateAssetMenu(fileName = "StageConfig", menuName = "Pukul Joko/Stage Config")]
public class StageConfig : ScriptableObject
{
    [Header("Identity")]
    [Range(1, 3)] public int stageIndex = 1;

    [Header("Round")]
    [Tooltip("Round length in seconds (before difficulty scaling).")]
    public float roundDuration = 30f;

    [Header("Spawnable Content")]
    [Tooltip("Only these archetypes may spawn during this stage. SpawnManager picks randomly among them.")]
    public WhackableConfig[] allowedTypes;

    [Tooltip("Optional per-type spawn weights, parallel to allowedTypes. " +
             "Leave empty for uniform random selection.")]
    public float[] spawnWeights;
}
