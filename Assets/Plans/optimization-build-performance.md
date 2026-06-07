# Optimization Plan: Build Size and Performance

## Goal
Reduce build size (currently ~100MB) and improve mobile performance by optimizing assets and settings.

## 1. Asset Cleanup
- Delete `Assets/TextMesh Pro/Examples & Extras/` (Large, unused in production).
- Delete `Assets/AI Toolkit/Temp/` (Temporary image references from AI generations).
- Delete `Assets/Settings/Lit2DSceneTemplate.scenetemplate` (Template not needed for build).
- Delete `Assets/Scenes/SampleScene.unity` (Template scene).

## 2. Audio Optimization
- **BGM**: Ensure `bgm_menu.mp3` and `bgm_gameplay.mp3` use **Streaming** load type and **Vorbis** compression.
- **SFX**: Set short effects (hit, click) to **Decompress On Load** but use **ADPCM** compression (less CPU heavy than Vorbis for short clips).

## 3. Texture Optimization
- Set **Max Texture Size** to 1024 for most UI and background textures.
- Ensure Android override uses **ASTC** compression (8x8 or 6x6).
- Disable **Generate Mip Maps** for UI-only textures.

## 4. Rendering Settings (URP)
- Disable **HDR** in the URP Asset (`Assets/Settings/UniversalRP.asset`).
- Reduce **Shadow Distance** to 30.
- Disable **Opaque Texture** and **Depth Texture** if not used by any shaders.

## 5. Build Settings
- Ensure **Managed Stripping Level** is set to **Medium** or **High**.
