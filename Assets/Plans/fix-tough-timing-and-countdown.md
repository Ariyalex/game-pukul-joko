# Project Overview
- Game Title: Pukul Joko (Whack-a-Mole)
- High-Level Concept: Whack targets to score points, avoid bombs, and handle "Tough" targets that require multiple hits.
- Target Platform: Standalone (Portrait)
- Render Pipeline: URP

# Game Mechanics
## WhackableTough Timing Fix
- Currently, "Tough" targets (requiring 2 hits) retreat based on their initial spawn timer even after the first hit. If hit late in their duration, they retreat during the "hit" animation, preventing the second hit.
- The fix involves resetting the retreat timer (`_autoHideTween`) in `Whackable.cs` whenever a non-lethal hit is registered.

## 3-Second Countdown
- Currently, the game starts immediately upon loading the GameplayScene.
- A 3-second countdown (3, 2, 1, GO!) will be added before the spawning begins and the game timer starts.

# UI
- **Countdown Text**: A new `TextMeshProUGUI` element in the `UIManager` will display the countdown in the center of the screen.

# Key Asset & Context
- `Assets/Scripts/Gameplay/Whackable.cs`: Handles hit logic and auto-hide timing.
- `Assets/Scripts/GameManager.cs`: Orchestrates the round start and state transitions.
- `Assets/Scripts/UIManager.cs`: Manages HUD and countdown display.

# Implementation Steps

## 1. Modify Whackable.cs to Reset Retreat Timer
- **Description**: Store the `stayDuration` during `Spawn` and restart the `_autoHideTween` when `OnHit` occurs but health remains.
- **Assigned role**: developer
- **Files**: `Assets/Scripts/Gameplay/Whackable.cs`
- **Dependencies**: None
- **Parallelizable**: Yes

## 2. Update UIManager for Countdown Support
- **Description**: Add a `countdownText` field and methods to show/update/hide the countdown.
- **Assigned role**: developer
- **Files**: `Assets/Scripts/UIManager.cs`
- **Dependencies**: None
- **Parallelizable**: Yes

## 3. Implement Countdown Logic in GameManager
- **Description**: Replace the immediate start in `Start()` with a Coroutine that handles the 3-second delay and UI updates before enabling spawning.
- **Assigned role**: developer
- **Files**: `Assets/Scripts/GameManager.cs`
- **Dependencies**: Step 2
- **Parallelizable**: No

## 4. Wire UI and Scene Setup
- **Description**: Add the `CountdownText` GameObject to the `HUD` in `GameplayScene` and assign it to the `UIManager` component.
- **Assigned role**: developer
- **Dependencies**: Step 3
- **Parallelizable**: No

# Verification & Testing
- **WhackableTough Test**: Play Stage 3 Hard. Hit a Tough character once near the end of its duration. Verify it stays up for an additional duration instead of retreating immediately.
- **Countdown Test**: Load the GameplayScene. Verify that "3... 2... 1... GO!" appears, the game timer doesn't move during this time, and spawning only begins after "GO!".
- **State Test**: Ensure the player cannot score or lose strikes during the countdown phase.
