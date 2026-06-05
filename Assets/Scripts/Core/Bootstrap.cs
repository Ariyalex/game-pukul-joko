using UnityEngine;

/// <summary>
/// Guarantees the persistent singletons (GameSession + AudioManager) exist exactly once.
/// Place this on a GameObject in MenuScene. If those managers are missing (e.g. you press
/// Play directly into GameplayScene during development), it spawns minimal fallbacks so the
/// game does not null-ref. Authored prefab instances in the scene are preferred.
/// </summary>
[DefaultExecutionOrder(-100)]
public class Bootstrap : MonoBehaviour
{
    [Tooltip("Optional authored prefab containing GameSession + AudioManager. " +
             "If left null, a bare GameSession is created as a fallback.")]
    [SerializeField] private GameObject persistentManagersPrefab;

    private void Awake()
    {
        // If a real persistent setup already exists, do nothing.
        if (GameSession.Instance != null) return;

        if (persistentManagersPrefab != null)
        {
            Instantiate(persistentManagersPrefab);
            return;
        }

        // Fallback: create a bare GameSession so scenes never null-ref on it.
        var go = new GameObject("GameSession (Fallback)");
        go.AddComponent<GameSession>();
    }
}
