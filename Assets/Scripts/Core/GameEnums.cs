/// <summary>
/// Project-wide enums shared across managers, configs and gameplay objects.
/// Kept in one file to avoid scattering tiny enum definitions.
/// </summary>
public enum Difficulty
{
    Normal,
    Hard
}

/// <summary>The three target archetypes the player can encounter.</summary>
public enum WhackableType
{
    Normal,   // 1 hit, grants score
    Penalty,  // bomb: tapping it costs a strike (3 strikes = game over)
    Tough     // 2 hits required to defeat
}

/// <summary>Which background music track should be playing.</summary>
public enum BgmType
{
    Menu,
    Gameplay
}

/// <summary>High-level gameplay flow states for a single round.</summary>
public enum GameState
{
    Intro,
    Playing,
    GameOver
}
