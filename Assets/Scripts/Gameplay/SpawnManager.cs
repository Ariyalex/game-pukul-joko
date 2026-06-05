using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drives the spawn loop. Owns the 9 holes. Each tick it picks a free hole and a random
/// archetype from the active stage's allowed set, applying the difficulty timing factor.
/// Spawning only runs while explicitly enabled by the GameManager.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private Hole[] holes; // exactly 9 (5 back + 4 front)

    [Header("Config")]
    [SerializeField] private DifficultyConfig difficultyConfig;

    private StageConfig _stage;
    private float _factor = 1f;        // difficulty timing multiplier
    private float _spawnTimer;
    private bool _running;

    // Scratch buffer reused to avoid per-tick allocations.
    private readonly List<int> _freeHoleIndices = new List<int>();

    /// <summary>Configures the loop for a round. Does not start it.</summary>
    public void Configure(StageConfig stage, Difficulty difficulty)
    {
        _stage = stage;
        _factor = difficultyConfig != null ? difficultyConfig.GetFactor(difficulty) : 1f;
        _spawnTimer = 0f;
    }

    public void StartSpawning() => _running = true;

    public void StopAll()
    {
        _running = false;
        if (holes != null)
            foreach (var h in holes) if (h != null) h.ForceReset();
    }

    private void Update()
    {
        if (!_running || _stage == null) return;

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer > 0f) return;

        TrySpawnOne();
        float interval = (difficultyConfig != null ? difficultyConfig.baseSpawnInterval : 1.2f) * _factor;
        _spawnTimer = interval;
    }

    private void TrySpawnOne()
    {
        if (holes == null || holes.Length == 0) return;

        // Gather free holes.
        _freeHoleIndices.Clear();
        for (int i = 0; i < holes.Length; i++)
            if (holes[i] != null && holes[i].CanSpawn) _freeHoleIndices.Add(i);

        if (_freeHoleIndices.Count == 0) return;

        WhackableConfig config = PickType();
        if (config == null) return;

        int holeIndex = _freeHoleIndices[Random.Range(0, _freeHoleIndices.Count)];
        float pop = (difficultyConfig != null ? difficultyConfig.basePopDuration : 0.25f) * _factor;
        float stay = (difficultyConfig != null ? difficultyConfig.baseStayDuration : 1.0f) * _factor;
        holes[holeIndex].Spawn(config, pop, stay);
    }

    /// <summary>Random archetype restricted to the active stage, honoring optional weights.</summary>
    private WhackableConfig PickType()
    {
        var types = _stage.allowedTypes;
        if (types == null || types.Length == 0) return null;

        var weights = _stage.spawnWeights;
        if (weights != null && weights.Length == types.Length)
        {
            float total = 0f;
            for (int i = 0; i < weights.Length; i++) total += Mathf.Max(0f, weights[i]);
            if (total > 0f)
            {
                float r = Random.value * total;
                for (int i = 0; i < types.Length; i++)
                {
                    r -= Mathf.Max(0f, weights[i]);
                    if (r <= 0f) return types[i];
                }
            }
        }

        return types[Random.Range(0, types.Length)];
    }
}
