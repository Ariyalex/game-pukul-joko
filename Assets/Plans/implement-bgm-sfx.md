# Project Overview
- Game Title: Pukul Joko
- High-Level Concept: A whack-a-mole style game where players hit normal and tough targets while avoiding penalty targets across multiple stages.
- Players: Single player
- Target Platform: Standalone Windows 64
- Render Pipeline: UniversalRP (URP)

# Game Mechanics
## Core Gameplay Loop
- Targets (Whackables) pop up from holes.
- Players tap targets to gain score or avoid penalty targets (bombs).
- Stages progress with increasing difficulty or different configurations.
- Game ends if time runs out or 3 strikes are accumulated.

## Audio Implementation
- **BGM**: Continuous looping background music for Menu and Gameplay scenes.
- **SFX**: Sound feedback for stage starts, target appearances, and hits.

# UI
- Menu Scene: Selection for difficulty and stages. BGM plays here.
- Gameplay Scene: Main game interface. BGM and SFX play here.

# Key Asset & Context
- **BGM Clips**: `bgm_menu.mp3`, `bgm_gameplay.mp3`.
- **SFX Clips**: `sound_start_stage_12.mp3`, `sound_start_stage_3.mp3`, `effect_normal_appear.mp3`, `effect_tough_appear.mp3`, `effect_hit.mp3`.
- **AudioManager**: Singleton handling BGM and SFX playback.
- **Whackable**: Handles target logic (appear/hit).
- **GameManager**: Handles stage flow and overall game state.

# Implementation Steps

## 1. Configure AudioManager Prefab
- **Description**: Update the `PersistentManagers` prefab (which contains `AudioManager`) to include the new audio assets.
- **Assigned role**: developer
- **Dependencies**: None
- **Details**:
    - Set `menuBGM` to `bgm_menu` (Assets/sound/bgm_menu.mp3).
    - Set `gameplayBGM` to `bgm_gameplay` (Assets/sound/bgm_gameplay.mp3).
    - Update `sfxList` with the following mappings:
        - `"StartStage12"`: `sound_start_stage_12.mp3`
        - `"StartStage3"`: `sound_start_stage_3.mp3`
        - `"NormalAppear"`: `effect_normal_appear.mp3`
        - `"ToughAppear"`: `effect_tough_appear.mp3`
        - `"Hit"`: `effect_hit.mp3`

## 2. Implement Whackable Appear Sounds
- **Description**: Modify `Whackable.cs` to play specific sounds when targets appear.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Details**:
    - In `Spawn()`, play `"NormalAppear"` for `WhackableType.Normal`.
    - In `Spawn()`, play `"ToughAppear"` for `WhackableType.Tough`.
    - Ensure no sound plays for `WhackableType.Penalty`.

## 3. Implement Whackable Hit Sounds
- **Description**: Modify `Whackable.cs` to play the hit sound for all target types.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Details**:
    - In `OnHit()`, replace existing `PlaySFX` calls (`"Bomb"`, `"Hit"`, `"Defeat"`) with `PlaySFX("Hit")`.
    - Ensure `effect_hit` plays for normal, tough (both hits), and penalty targets.

## 4. Implement Stage Start Sounds
- **Description**: Modify `GameManager.cs` to play the appropriate stage start sound.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Details**:
    - In `Start()`, check `_activeStage.stageIndex` (or calculate from `GameSession.Instance.SelectedStage`).
    - Play `"StartStage12"` if stage is 1 or 2.
    - Play `"StartStage3"` if stage is 3.

## 5. Remove Redundant Audio Calls
- **Description**: Clean up any other old audio calls that might conflict.
- **Assigned role**: developer
- **Dependencies**: Step 3
- **Details**:
    - Check `GameManager.RegisterPenalty()` and remove or update `PlaySFX("Strike")` if it overlaps with the whackable hit sound. Since `Whackable.OnHit` will now handle the hit sound for penalties, we can remove it from `RegisterPenalty`.

# Verification & Testing
- **BGM Check**: Enter Menu Scene, verify `bgm_menu` loops. Load Gameplay Scene, verify `bgm_gameplay` loops.
- **Stage Start Check**: Start Stage 1 or 2, verify `sound_start_stage_12` plays. Start Stage 3, verify `sound_start_stage_3` plays.
- **Appear Sound Check**:
    - Verify `effect_normal_appear` plays when a normal whackable pops up.
    - Verify `effect_tough_appear` plays when a tough whackable pops up.
    - Verify NO sound plays when a penalty whackable pops up.
- **Hit Sound Check**:
    - Tap a normal whackable -> verify `effect_hit` plays.
    - Tap a tough whackable (1st hit) -> verify `effect_hit` plays.
    - Tap a tough whackable (2nd hit) -> verify `effect_hit` plays.
    - Tap a penalty whackable -> verify `effect_hit` plays.
