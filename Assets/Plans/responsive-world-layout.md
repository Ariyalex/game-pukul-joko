# Project Overview
- Game Title: Pukul Joko
- High-Level Concept: Whack-a-mole game with responsive camera and background.
- Players: Single Player
- Target Platform: Android (Portrait)
- Screen Orientation: Portrait (various aspect ratios)
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
- Targets pop from a grid of holes.
- The entire grid must remain visible regardless of screen aspect ratio.
- The background should always fill the screen without gaps.

# UI
- HUDCanvas (already using CanvasScaler) will remain overlaid.

# Key Asset & Context
- **HoleGrid**: The gameplay area that needs to be fitted horizontally. (Width: ~10.4 units).
- **Ground**: The background sprite that needs to cover the view. (Size: ~10.35 x 14 units).
- **Main Camera**: Standard Orthographic camera.

# Implementation Steps

## Step 1: Create CameraHorizontalFitter script
- **Description**: Create `Assets/Scripts/CameraHorizontalFitter.cs`. This script ensures the camera's orthographic size is adjusted so a specific horizontal world width is always visible.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

## Step 2: Create BackgroundAspectFiller script
- **Description**: Create `Assets/Scripts/BackgroundAspectFiller.cs`. This script scales the `Ground` object so it always covers the camera's full field of view (Aspect Fill).
- **Assigned role**: developer
- **Dependencies**: Step 1 (needs final camera size)
- **Parallelizable**: No

## Step 3: Setup Scene Components
- **Description**: 
    1. Attach `CameraHorizontalFitter` to `Main Camera` in `GameplayScene`.
    2. Set `Target Width` to `11.0`.
    3. Attach `BackgroundAspectFiller` to `Ground`.
- **Assigned role**: developer
- **Dependencies**: Step 1, Step 2
- **Parallelizable**: No

# Verification & Testing
- **Editor Verification**: Change Game View aspect ratio between 9:16, 9:21, and 3:4.
- **HoleGrid Bounds**: Verify that `Hole_Back_0` and `Hole_Back_4` (the edge holes) are never cropped.
- **Ground Coverage**: Verify no "Clear Color" (background color) is visible at the screen edges.
