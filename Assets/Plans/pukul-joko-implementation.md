# Project Overview
- **Game Title**: Pukul Joko
- **High-Level Concept**: A classic Whack-a-Mole style mobile game where players tap on "Joko" (the target) to score points while avoiding bombs in higher difficulties.
- **Players**: Single player (Mobile/Desktop)
- **Inspiration / Reference Games**: Whack-a-Mole
- **Tone / Art Direction**: Casual, colorful, and fun.
- **Target Platform**: Android + iOS (and PC for testing)
- **Screen Orientation**: Portrait 1080x1920
- **Render Pipeline**: URP (Universal Render Pipeline)

# Game Mechanics
## Core Gameplay Loop
1. Targets (Joko) pop up from holes randomly.
2. Player taps on Joko to score points (+10).
3. Hits trigger a "Hit" sprite and star particles.
4. Difficulty increases with stages (more holes) and hard mode (bombs spawn).
5. Game ends when the 30-second timer reaches zero.

## Controls and Input Methods
- **Touch/Click**: Uses Physics2D Raycasting to detect taps on Joko or Bombs.
- **New Input System**: Handling screen taps/clicks consistently across mobile and PC.

# UI
- **Intro/Main Menu**: Title text, "Start" button, "Settings", "Exit".
- **HUD**: Score display (top-left), Timer display (top-right).
- **Game Over**: Final Score, "Restart" button, "Main Menu" button.

# Key Asset & Context
- **Scripts**: `GameManager.cs`, `JokoController.cs`, `UIManager.cs`, `AudioManager.cs`.
- **Prefabs**: `JokoPrefab` (Sprite + Collider + JokoController), `HolePrefab`.
- **Library**: DOTween (for smooth movement and scaling).

# Implementation Steps

## Phase 1: Asset Sourcing & Placeholder Setup
1. **Placeholder Assets**:
   - `Background`: Square sprite scaled to fit screen.
   - `Hole`: Circle sprite (Dark grey).
   - `JokoNormal`: Square sprite (Brown).
   - `JokoHit`: Square sprite (Red/Benjol).
   - `Bomb`: Circle sprite (Black/Red).
   - `StarParticle`: Default Unity Particle System.
2. **Audio Recommendations**:
   - BGM: "Cheerful" tracks from *OpenGameArt.org*.
   - SFX: "Pop", "Hit", "Explosion" from *Freesound.org*.

## Phase 2: Unity Editor Hierarchy & UI Setup
1. **Scene Structure**:
   - `Main Camera`: Orthographic, Size adjusted for portrait.
   - `Environment`: Parent for Background and Holes.
   - `SpawnPoints`: A grid layout for holes (2x2, 2x3, or 3x3 depending on stage).
2. **UI Canvases**:
   - `MenuCanvas`: Enabled on start.
   - `GameHUD`: Enabled during "Playing" state.
   - `GameOverCanvas`: Enabled on game end.

## Phase 3: Library Integration (DOTween)
1. **Setup**: Import DOTween from the Asset Store.
2. **Logic**: Use `transform.DOLocalMoveY(targetY, duration)` for the "pop up" and "hide" animations. Use `transform.DOScale` for a punchy hit effect.

## Phase 4: Complete C# Scripts
1. **GameManager.cs**:
   - Manage `GameState` enum.
   - Timer logic (30s).
   - Stage management (Hole count).
   - Spawn logic based on difficulty (Normal/Hard).
2. **JokoController.cs**:
   - Handle Raycast detection via `InputSystem`.
   - DOTween sequences for PopUp/Down.
   - Handle "Hit" vs "Bomb" logic.
3. **UIManager.cs**:
   - Reference all Canvases and Text elements.
   - Methods to `ShowMenu()`, `StartGame()`, `ShowGameOver()`.
4. **AudioManager.cs**:
   - Singleton pattern.
   - `AudioSource` for BGM and `PlayOneShot` for SFX.

# Verification & Testing
1. **State Transition Test**: Start Menu -> Playing -> Game Over.
2. **Input Test**: Verify clicks/taps register on Joko and trigger Hit state.
3. **Difficulty Test**: Verify Stage 1/2/3 hole counts and Bomb spawning in Hard mode.
4. **Audio Test**: Ensure SFX plays on hit and BGM switches correctly.
5. **DOTween Test**: Verify smooth movement of Joko.
