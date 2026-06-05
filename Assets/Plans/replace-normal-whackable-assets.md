# Project Overview
- Game Title: Pukul Joko
- High-Level Concept: Whack-a-Mole game with character-based targets.
- Goal: Replace the "Normal Object" (1-hit) placeholders with the provided character art.

# Game Mechanics
## Visual Feedback
- **Normal State**: Sweating character (Item 1).
- **Hit State**: Character with "Anggaran" helmet and crossed eyes (Item 2).

# Key Asset & Context
- **Original Normal Texture**: InstanceID=97410
- **Original Hit Texture**: InstanceID=97418
- **Target Configuration**: `Assets/Configs/Whackable_Normal.asset`
- **Pivot**: BottomCenter (required for the "emerge from hole" animation).

# Implementation Steps
1. **Background Removal**:
   - The provided images have solid black backgrounds. 
   - Remove the black backgrounds from Item 1 and Item 2 to create transparent character sprites.
   - **Assigned role**: developer
   - **Parallelizable**: Yes

2. **Sprite Configuration**:
   - Set the `Texture Type` to `Sprite (2D and UI)`.
   - Set `Pivot` to `BottomCenter`.
   - **Assigned role**: developer
   - **Parallelizable**: Yes

3. **Update ScriptableObject & Scale Calibration**:
   - Open `Assets/Configs/Whackable_Normal.asset`.
   - Assign the processed Item 1 to `normalSprite`.
   - Assign the processed Item 2 to `hitSprite`.
   - Reset `greyboxColor` and `hitColor` to White (1,1,1,1).
   - **Scale Matching**: The current capsule placeholder is approximately 1.0 units high. I will adjust the `Pixels Per Unit` (PPU) of the new sprites or apply a local scale to the `Whackable` object to ensure the head size fits within the hole rims without overwhelming the scene.
   - **Assigned role**: developer
   - **Parallelizable**: No

4. **Scene & Prefab Validation**:
   - Verify character scale in `GameplayScene`.
   - Adjust `Hole` prefab masking if the new character height differs significantly from the placeholder.
   - **Assigned role**: developer
   - **Parallelizable**: No

# Verification & Testing
1. **Editor Inspection**: Confirm sprites have transparent backgrounds and correct pivots in the Inspector.
2. **Gameplay Test**: 
   - Run Stage 1 (which only uses the Normal Object).
   - Verify the character pops up correctly (bottom-aligned).
   - Verify hitting the character triggers the visual swap to the helmet state before it hides.
