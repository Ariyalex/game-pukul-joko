# Project Overview
- Game Title: Pukul Joko
- High-Level Concept: Whack-a-mole game (Portrait orientation).
- Current Issue: UI element scaling is mismatched due to inconsistent Pixels Per Unit (PPU) settings. The user attempted to map PPU to image height, causing scaling issues relative to the background.

# Game Mechanics
(No changes to core gameplay loop)

# UI
- MenuScene UI needs scale normalization.
- Goal: All UI elements should have consistent scaling relative to each other and the background, following standard Unity UI conventions (100 PPU).

# Key Asset & Context
- Title: `Assets/AI Toolkit/Temp/AssistantImageReferences/2e3d71fd9c11d17a5db6983f4f50ba1ea26c817f.png` (2048x770)
- Difficulty BG: `Assets/AI Toolkit/Temp/AssistantImageReferences/2b528e8a6841be6003dbebb62de56479f52c7cec.png` (2048x1051)
- Normal Button: `Assets/AI Toolkit/Temp/AssistantImageReferences/5b624182d1ff46f8e1164debb25db9abd7910b68.png` (2036x1369)
- Hard Button: `Assets/AI Toolkit/Temp/AssistantImageReferences/7124ed523dd6498b060671d07b9abc9b070f210b.png` (1999x1395)
- Stage Label: `Assets/AI Toolkit/Temp/AssistantImageReferences/b56eb72e7a3bdceeafc808efd8c9b449e1511642.png` (2048x386)
- Background: `Assets/AI Toolkit/Temp/AssistantImageReferences/5ab2864f64115ef9aa06790bbd311af735f73bab.png` (572x1024)

# Implementation Steps
## 1. Normalize Sprite PPU
- **Action**: Change the `Pixels Per Unit` setting for all the above sprites to **100** in their `TextureImporter`.
- **Reason**: Standardizing to 100 PPU ensures that pixel dimensions translate predictably to UI units when using a `CanvasScaler` with 100 Reference PPU.
- **Assigned role**: developer
- **Parallelizable**: Yes

## 2. Update Canvas Configuration
- **Action**: Ensure the `MenuCanvas` has a `CanvasScaler` set to:
    - UI Scale Mode: **Scale With Screen Size**
    - Reference Resolution: **1080 x 1920** (Portrait)
    - Reference Pixels Per Unit: **100**
- **Assigned role**: developer
- **Parallelizable**: No

## 3. Re-scale UI Elements in MenuScene
- **Action**: For each UI element (`Title`, `DiffLabel`, `NormalButton`, `HardButton`, `StageLabel`):
    1. Reset Scale to **(1, 1, 1)**.
    2. Call **Set Native Size** on the `Image` component.
    3. Since the images are high-resolution (approx 2048 wide) and the canvas is 1080 wide, scale them down to fit (approx **0.4x - 0.5x** scale).
    4. Adjust positions to match the intended design (Top-Center anchoring).
- **Background**: Ensure `MenuBackground` is anchored to stretch (0,0 to 1,1) and `Preserve Aspect` is enabled.
- **Assigned role**: developer
- **Dependencies**: Step 1 & 2
- **Parallelizable**: No

## 4. Fix DOTween Animation Logic
- **Action**: Since we are changing the base scale of buttons to ~0.5, update `MenuUIManager.cs` to handle the transition relative to the new base scale instead of assuming a base scale of 1.0.
- **Assigned role**: developer
- **Dependencies**: Step 3

# Verification & Testing
- Open `MenuScene` and verify that the layout looks proportional.
- Test in Game View with different resolutions (e.g., 9:16, 10:16) to ensure scaling remains consistent.
- Enter Play Mode and verify that buttons scale up correctly (e.g., from 0.5x to 0.6x) when selected.
