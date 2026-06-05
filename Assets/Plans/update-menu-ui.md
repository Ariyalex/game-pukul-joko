# Project Overview
- Game Title: Pukul Joko
- High-Level Concept: Whack-a-mole style game with penalty mechanics and difficulty levels.
- Players: Single player
- Inspiration / Reference Games: Whack-a-mole
- Tone / Art Direction: Cartoon / Playful
- Target Platform: Standalone Windows 64
- Screen Orientation / Resolution: Landscape 1920x1080
- Render Pipeline: URP

# Game Mechanics
## Core Gameplay Loop
- Players select difficulty and stage in the menu.
- In gameplay, targets pop up from holes.
- Tapping normal/tough targets grants score.
- Tapping penalty targets costs a strike.
- Surviving until the timer ends or losing 3 strikes.

## Controls and Input Methods
- Mouse/Touch input to whack targets.
- Menu navigation via buttons.

# UI
The MenuScene UI will be overhauled to match the new visual style:
- Background matching the gameplay scene.
- Graphical title and labels instead of text.
- Enhanced selection feedback for difficulty and stage buttons.

# Key Asset & Context
- Item 1 (Title): `Assets/AI Toolkit/Temp/AssistantImageReferences/2e3d71fd9c11d17a5db6983f4f50ba1ea26c817f.png`
- Item 2 (Difficulty BG): `Assets/AI Toolkit/Temp/AssistantImageReferences/2b528e8a6841be6003dbebb62de56479f52c7cec.png`
- Item 3 (Normal Button): `Assets/AI Toolkit/Temp/AssistantImageReferences/5b624182d1ff46f8e1164debb25db9abd7910b68.png`
- Item 4 (Hard Button): `Assets/AI Toolkit/Temp/AssistantImageReferences/7124ed523dd6498b060671d07b9abc9b070f210b.png`
- Item 5 (Select Stage Label): `Assets/AI Toolkit/Temp/AssistantImageReferences/b56eb72e7a3bdceeafc808efd8c9b449e1511642.png`
- Gameplay Background: `Assets/AI Toolkit/Temp/AssistantImageReferences/5ab2864f64115ef9aa06790bbd311af735f73bab.png`

# Implementation Steps
## 1. Visual Asset Updates in MenuScene
- **Background**: Add an `Image` object `MenuBackground` to the `MenuCanvas`. Set it as the first child to be behind other elements. Use the gameplay background sprite. Use **Set Native Size** to ensure pixel-perfect scale.
- **Title**: Replace the `TextMeshProUGUI` on the `Title` object with an `Image` component using Item 1. Use **Set Native Size** to match image pixel dimensions exactly.
- **Difficulty Panel**:
    - Remove the `TextMeshProUGUI` from `DiffLabel`.
    - Add an `Image` component to `DiffLabel` using Item 2.
    - Use **Set Native Size** and then position it to encompass the `NormalButton` and `HardButton`.
- **Difficulty Buttons**:
    - Update `NormalButton`'s `Image` sprite to Item 3. Use **Set Native Size**. Remove its child `Text (TMP)`.
    - Update `HardButton`'s `Image` sprite to Item 4. Use **Set Native Size**. Remove its child `Text (TMP)`.
- **Select Stage Label**:
    - Replace the `TextMeshProUGUI` on `StageLabel` with an `Image` component using Item 5. Use **Set Native Size**.
- **Glow Shadow Setup**:
    - For each button (`NormalButton`, `HardButton`, and all `stageButtons`), add a child `Image` named `GlowEffect`.
    - Use a blurred circle or a tinted version of the button sprite (if possible, or a simple white square with low alpha). Set to inactive by default.

## 2. Menu Logic & Animation
- **Script Update**: Modify `Assets/Scripts/MenuUIManager.cs`.
    - Replace the `Tint()` logic with a DOTween-based `AnimateSelection()` method.
    - Add variables for `selectedScale`, `selectedYOffset`, and `animationDuration`.
    - The method will:
        - Scale the selected button up.
        - Move its local Y position up by the offset.
        - Enable/Disable the `GlowEffect` child.
        - Reset these properties for unselected buttons.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: No

# Verification & Testing
- Open `MenuScene`.
- Verify all images are correctly assigned and look sharp.
- Enter Play Mode.
- Click "Hard" difficulty: verify it scales up, moves up, and shows a glow. Verify "Normal" resets.
- Click "Stage 2": verify the same feedback applies.
- Click "Play" and ensure the game loads with the correct settings.
