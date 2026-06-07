# Project Overview
- **Game Title**: Pukul Joko
- **High-Level Concept**: Whack-a-mole game.
- **Requirement**: Change `HUDCanvas` to be fixed in the world (like `HoleGrid` and `Ground`) and not relative to screen size scaling.

# Game Mechanics
## Audio/UI Logic
- The HUD currently overlays the screen and scales with resolution.
- The user wants the HUD to be part of the world space, ensuring its position is absolute and "fixed" regardless of screen aspect ratio changes (though it will stay within the world coordinates).

# Key Asset & Context
- **HUDCanvas**: Currently `ScreenSpaceOverlay` with `ScaleWithScreenSize`.
- **Main Camera**: Orthographic, size 7.
- **World Dimensions**: Height = 14 units, Width ≈ 7.8 units (at 1080:1920 portrait).

# Implementation Steps

## 1. Convert HUDCanvas to World Space
- **Description**: Change the `Canvas` component on `HUDCanvas` to `RenderMode.WorldSpace`.
- **Assigned role**: developer
- **Dependencies**: None
- **Details**:
    - Set `RenderMode` to `WorldSpace`.
    - Assign `Main Camera` to the `Event Camera` slot.

## 2. Standardize Canvas Transform
- **Description**: Set the `RectTransform` of `HUDCanvas` to a fixed world size and scale.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Details**:
    - Set `Position` to `(0, 0, 0)`.
    - Set `Rotation` to `(0, 0, 0)`.
    - Set `Scale` to `(0.01, 0.01, 0.01)`. This makes 100 UI pixels = 1 World unit.
    - Set `Width` to `1080` and `Height` to `1920` (matching the previous reference resolution).

## 3. Remove CanvasScaler
- **Description**: Since the Canvas is now in World Space, the `CanvasScaler` in "Scale with Screen Size" mode is no longer needed or appropriate.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Details**:
    - Remove the `CanvasScaler` component or change its mode to `Constant Pixel Size` (which has no effect in World Space but prevents scaling conflicts).

## 4. Reposition HUD Elements
- **Description**: Adjust the positions of the children (`StageInformation`, `TimerInformation`, `Strike_X`) to ensure they are visible within the Camera's orthographic view (Size 7).
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Details**:
    - With a scale of 0.01 and Canvas at (0,0,0), the viewable area is roughly `X: [-390, 390]` and `Y: [-700, 700]` (depending on aspect ratio).
    - Move `TimerInformation` and `StageInformation` towards the top (`Y ≈ 600`).
    - Move `Strike` icons to their desired world positions.

# Verification & Testing
- **Visual Check**: In the Unity Editor Scene View, the `HUDCanvas` should appear right on top of the `Ground` or `HoleGrid`.
- **Resolution Change**: Change the Game View resolution (e.g., from 1080x1920 to something else). The UI elements should stay fixed relative to the holes, not "floating" or stretching relative to the screen edges.
- **Raycast Check**: Ensure buttons (if any) still work by checking the `GraphicRaycaster` on the Canvas.
