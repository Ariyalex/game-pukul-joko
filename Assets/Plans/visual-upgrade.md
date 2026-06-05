# Project Overview
- Game Title: Pukul Joko
- High-Level Concept: Whack-a-mole style game with improved 2D visuals based on reference art.
- Players: Single player
- Tone / Art Direction: Bright, casual, 2D vector style.
- Render Pipeline: URP 2D

# Game Mechanics
## Core Gameplay Loop
- Logic remains: Randomly spawn targets, tap to score, avoid bombs in hard mode.
## Visual Enhancements
- **Environment**: Multi-layered background consisting of a sky and a green hill.
- **Perspective**: Staggered layout of holes to mimic the reference image's depth.
- **Depth Effect**: Using sorting layers to make the "Joko" targets appear from behind the ground line of the holes.

# UI
- Menu, HUD, and Game Over screens already implemented; will ensure they remain functional and clear over the new background.

# Key Asset & Context
- **Sky**: (Removed to simplify background to full green)
- **GrassHill**: Full-screen green sprite representing the play area.
- **Hole Sprite**: Dark oval sprite (Enlarged).
- **Joko Template**: A rounded square sprite (Mole brown placeholder) that pops up from the holes.
- **Sorting Layers**:
    - `Background`: Grass full screen
    - `Holes`: The hole ovals
    - `Targets`: Joko (The moles)
    - `UI`: Score/Timer HUD

# Implementation Steps
1. **Background Overhaul**:
    - Set Camera background color to Vibrant Green.
    - Remove the "Sky" object.
    - Scale the "Grass" object to cover the full screen (e.g., scale 100, 100) and set its color to match the camera background.
2. **Hole Size Upgrade**:
    - Increase the scale of all `Hole_X` parent objects significantly (multiplying current scales by 3).
3. **Bug Fix: Masking & Visibility**:
    - Reposition the `MoleMask` in each hole so its bottom edge aligns with the hole opening. (Current masks are too high at Y=8.5, moles are at Y=-0.7).
    - Update `JokoController` internal variables `hideY` and `showY` to match the new larger scale (e.g., hide at -2.5, show at 1.2).
4. **Layout & Perspective Adjustment**:
    - Maintain the staggered layout but ensure holes don't overlap awkwardly with the new larger size.

# Verification & Testing
- **Visual Check**: Does the mole look like it's inside the hole? Is the sky and grass visible?
- **Gameplay Check**: Can we still tap the moles accurately?
- **Sorting Check**: Ensure UI elements (Score/Timer) are always on top.
