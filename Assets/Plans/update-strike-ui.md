# Project Overview
- Game: Pukul Joko
- Task: Update the Strike icons UI logic to swap sprites instead of colors.

# Game Mechanics
## Strike System
- The game tracks up to 3 strikes (penalties).
- Currently, strike icons change color (red for filled, white-alpha for empty).
- The user wants to use specific sprites:
    - Normal (no penalty): `1780739323383.png`
    - Penalty: `1780739351214.png`
- The icons should always be white (no color overlay).

# Key Asset & Context
- `Assets/Scripts/UIManager.cs`: Manages the HUD and strike icons.
- `Strike_0`, `Strike_1`, `Strike_2`: UI Image objects in the `HUDCanvas`.
- `Assets/AI Toolkit/Temp/AssistantImageReferences/1780739323383.png` (Sprite 4)
- `Assets/AI Toolkit/Temp/AssistantImageReferences/1780739351214.png` (Sprite 5)

# Implementation Steps

## 1. Modify UIManager.cs
- **Description**: Add sprite fields and update the `SetStrikes` method to swap sprites and set color to white.
- **Assigned role**: developer
- **Dependencies**: None
- **Details**:
    - Add `[SerializeField] private Sprite normalStrikeSprite;`
    - Add `[SerializeField] private Sprite penaltyStrikeSprite;`
    - Update `SetStrikes(int strikes)` to assign the appropriate sprite based on the index.
    - Ensure `strikeIcons[i].color = Color.white;` is set.

## 2. Update HUDCanvas in GameplayScene
- **Description**: Assign the new sprites to the `UIManager` component and reset icon colors.
- **Assigned role**: developer
- **Dependencies**: Step 1
- **Details**:
    - Find the `HUDCanvas` GameObject.
    - Assign the `normalStrikeSprite` (Item 4) and `penaltyStrikeSprite` (Item 5) to the `UIManager` component.
    - Set the color of the `Strike_0`, `Strike_1`, and `Strike_2` Image components to white.
    - Call `SetStrikes(0)` or similar to initialize the visuals.

# Verification & Testing
- Enter Play Mode.
- Trigger a penalty (hit a bomb).
- Verify the strike icon changes from the normal sprite to the penalty sprite.
- Verify the color is white (no red overlay).
