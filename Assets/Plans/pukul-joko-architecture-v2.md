# Pukul Joko — Development Plan & System Architecture (v2)

> This plan supersedes the legacy `pukul-joko-implementation.md` (single-scene prototype). It is a near-total
> architecture overhaul driven by the new technical constraints: two scenes, type-driven stage progression,
> a pseudo-3D 9-hole grid, a centralized input manager, a 3-strike penalty system, and DOTween-only motion.
> The existing scripts (`GameManager`, `JokoController`, `UIManager`, `AudioManager`) will be **refactored/replaced**,
> not extended in place.

---

# Project Overview
- **Game Title**: Pukul Joko (Whack-a-Mole)
- **High-Level Concept**: A snappy mobile whack-a-mole. Tap "Joko" targets popping from 9 oval holes to score, while later stages introduce bombs (penalties) and armored targets that need two hits.
- **Players**: Single player.
- **Inspiration / Reference Games**: Classic Whack-a-Mole arcade cabinets, mobile reflex tappers.
- **Tone / Art Direction**: Casual, colorful, juicy feedback. **Greybox first** with Unity default 2D primitives (Circle/Square/Capsule), architected for later `SpriteRenderer.sprite` swapping.
- **Target Platform**: Android + iOS (PC/Editor for testing).
- **Screen Orientation / Resolution**: Portrait 1080x1920.
- **Render Pipeline**: URP (Universal RP 17.4.0, already active).
- **Unity Version**: 6000.4.8f1. **New Input System** (1.19.0). **DOTween** already present at `Assets/Plugins/Demigiant/DOTween`.

---

# Game Mechanics

## Core Gameplay Loop
1. Player selects **Difficulty** (Normal/Hard) and a **Stage** (1–3) in `MenuScene`, then loads `GameplayScene`.
2. A 30-second round begins. Targets pop up from the 9 oval holes at intervals.
3. Player taps targets:
   - **Normal Object** — 1 tap → score + hides immediately + star particles.
   - **Penalty Object (Bomb)** — tapping it = +1 strike. **3 strikes → immediate Game Over.**
   - **Tough Object** — exactly 2 taps. Hit 1 = visual feedback (sprite swap + shake). Hit 2 = defeat + score + stars.
4. Round ends on **timer = 0** (Win/Time-Up) or **3 strikes** (Lose). Game Over UI shows the result + final score.
5. Player restarts the same stage or returns to the menu.

**Stage content (discrete, selectable):** Each stage is a standalone 30s round.
| Stage | Object types that can spawn |
|-------|------------------------------|
| 1 | Normal only |
| 2 | Normal + Penalty (Bomb) |
| 3 | Normal + Penalty + Tough (all three, random) |

## Controls and Input Methods
- **Mobile touch + Editor mouse**, unified via the New Input System `Pointer.current` (covers both touch and mouse).
- On press, a **single** `Physics2D.Raycast` is fired from `Camera.ScreenToWorldPoint(pointerPos)` with `Vector2.zero` direction; the hit `Collider2D`'s `Whackable` (if any) receives `OnHit()`.
- Each spawnable object has a `BoxCollider2D` sized to its sprite. **No per-object Update polling** (the legacy `JokoController` did this 9× per frame — removed).

---

# UI

## MenuScene (Portrait wireframe)
```
+--------------------------------+
|            PUKUL JOKO          |   <- Title (TMP)
|                                |
|   DIFFICULTY:  [ Normal ] [Hard]|  <- toggle group (one selected)
|                                |
|   SELECT STAGE:                |
|     [ Stage 1 ] [ Stage 2 ]    |  <- stage buttons
|     [ Stage 3 ]                |
|                                |
|          [   PLAY   ]          |  <- loads GameplayScene with chosen settings
|          [   QUIT   ]          |
+--------------------------------+
```
- Difficulty = exclusive toggle (Normal default). Stage = 3 buttons that set the selected stage (highlight selection).
- `PLAY` is enabled once a stage is chosen; writes to `GameSession` then `SceneManager.LoadScene("GameplayScene")`.
- Menu BGM plays here (persistent `AudioManager`).

## GameplayScene (Portrait wireframe)
```
+--------------------------------+
| Score: 0        Stage 1        |  <- HUD top bar (TMP)
| Strikes: [o][o][o]   Time: 30  |  <- strike icons + countdown
|                                |
|        (back row: 5 holes)     |
|       O   O   O   O   O         |  <- oval holes (Circle scaled Y=0.4)
|         O   O   O   O           |  <- front row: 4 holes (staggered, lower, drawn over back)
|                                |
+--------------------------------+
            Game Over overlay:
+--------------------------------+
|        TIME UP! / GAME OVER    |
|        Final Score: 123        |
|        [ RETRY ]  [ MENU ]     |
+--------------------------------+
```
- HUD: Score (TMP), Stage label, **3 strike icons** (filled/empty), Time countdown.
- Strike icons are simple Image objects; `UIManager.SetStrikes(n)` toggles their state.
- Game Over overlay shows "TIME UP!" (win) or "GAME OVER" (3 strikes) plus final score and Retry/Menu buttons.
- Gameplay BGM plays here.

---

# Pseudo-3D Grid Specification (9 holes)

- **Hole visual** = Unity default Circle sprite, scaled to a flat oval: `localScale ≈ (1.6, 0.6, 1)` (X wide, Y squashed ~0.4 ratio) to fake a 25° top-down view.
- **Layout** (local positions under a `HoleGrid` parent; tune in editor):
  - **Back row (5)**: `y = +1.0`, `x = {-4, -2, 0, 2, 4}`
  - **Front row (4)**: `y = -1.5`, `x = {-3, -1, +1, +3}` (staggered between back holes), and scaled slightly larger (e.g. ×1.15) for depth.
- **Per-hole structure (prefab `Hole`)**:
  - `HoleBack` SpriteRenderer (the oval ground opening) — Sorting Layer **HoleBack**.
  - `SpawnAnchor` (empty transform at oval center) — the `Whackable` is parented/positioned here.
  - `HoleFront` SpriteRenderer (front rim crescent oval) — Sorting Layer **HoleFront** — draws OVER the lower part of the object so it appears to emerge from inside the hole.
- **Sorting Layers** (create in Project Settings → Tags and Layers, back→front order):
  `Background` → `HoleBack` → `Object` → `HoleFront` → (`UI` via Canvas).
- **Row ordering within layers**: front-row renderers use a higher `sortingOrder` than back-row so the front row overlaps the back row. Suggested: back row `sortingOrder = 0`, front row `sortingOrder = 10`, applied consistently to that hole's HoleBack/Object/HoleFront.
- **Pop motion**: object sits hidden at `SpawnAnchor.y - popHeight`; `DOMoveY` lifts it to `SpawnAnchor.y + popHeight`; the `HoleFront` masks its lower half throughout, selling the "emerge from hole" effect.

---

# Key Asset & Context

## Placeholder (greybox) assets
| Asset | Primitive | Notes |
|-------|-----------|-------|
| Background | Square sprite | Scaled to fill portrait camera; muted color. |
| Hole (back/front) | Circle sprite | Scaled to oval; HoleBack dark grey, HoleFront slightly lighter rim. |
| Normal Object | Square/Capsule | Brown. `normalSprite` → `hitSprite` swap ready. |
| Penalty Object (Bomb) | Circle | Black/red. |
| Tough Object | Capsule | Grey "armor"; `normalSprite` → `damagedSprite` → defeat. |
| Star Particle | ParticleSystem | Default Unity PS placeholder on defeat; one instance per Whackable or a pooled `HitFXPlayer`. |

## ScriptableObjects (data-driven config — recommended)
- **`WhackableConfig`** (one asset per object type): `WhackableType type`, `Sprite normalSprite`, `Sprite hitSprite`, `int hitsRequired`, `int scoreValue`, `bool isPenalty`, `Color greyboxColor`.
- **`StageConfig`** (one asset per stage 1–3): `int stageIndex`, `float roundDuration` (30), `WhackableConfig[] allowedTypes`, optional spawn weights.
- **`DifficultyConfig`**: `float baseSpawnInterval` (1.2), `float basePopDuration` (0.25), `float baseStayDuration` (1.0), `float hardMultiplier` (≈0.6 → "significantly faster"). Hard multiplies all three (smaller = faster). This satisfies "the ONLY difference is the multiplier."

## C# Scripts & Responsibilities

### Persistent (DontDestroyOnLoad, created by Bootstrap in MenuScene)
- **`GameSession.cs`** (singleton) — Holds the player's choices that travel between scenes:
  - `Difficulty SelectedDifficulty` (enum `Normal`/`Hard`)
  - `int SelectedStage` (1–3)
  - `float DifficultyFactor` (1.0 Normal, `hardMultiplier` Hard)
  - No gameplay logic; pure data carrier. Survives scene loads.
- **`AudioManager.cs`** (singleton, refactor of existing) — `bgmSource`, `sfxSource`, `menuBGM`, `gameplayBGM`, `Dictionary<string,AudioClip>` SFX (Pop/Hit/Defeat/Bomb/Strike). `PlayBGM(BgmType)`, `PlaySFX(string)`. Crossfade optional via DOTween (`bgmSource.DOFade`).
- **`Bootstrap.cs`** — Ensures `GameSession` + `AudioManager` exist exactly once (spawn-if-missing in MenuScene `Awake`).

### MenuScene
- **`MenuUIManager.cs`** — Wires difficulty toggles + stage buttons → writes to `GameSession`; `PLAY` loads `GameplayScene`; `QUIT` quits. Triggers menu BGM.

### GameplayScene
- **`GameManager.cs`** (singleton, scene-scoped) — **The orchestrator.** Owns `GameState` (`Intro`, `Playing`, `GameOver`), the 30s timer, score, and the **3-strike counter**. Reads `GameSession` on Start to pick the active `StageConfig` and `DifficultyFactor`. Public API: `AddScore(int)`, `RegisterPenalty()`, `RegisterDefeat()`, `EndGame(bool won)`. (See "3-Stage & 3-Strike Logic" below.)
- **`SpawnManager.cs`** — Holds the 9 `Hole` references. Runs the spawn loop (coroutine/`DOVirtual` timer). Each tick: pick a free hole + a random `WhackableConfig` from the active stage's `allowedTypes`; call `hole.Spawn(config, popDuration, stayDuration)`. Applies `DifficultyFactor` to interval/pop/stay. Pauses when not `Playing`. Pooling: reuse one `Whackable` per hole.
- **`Hole.cs`** — Represents one grid hole: refs to `HoleBack`, `HoleFront`, `SpawnAnchor`, and its pooled `Whackable`. Tracks `IsOccupied`. `Spawn(config,...)` activates/configures the Whackable and triggers pop. Assigns correct sorting orders per row.
- **`Whackable.cs`** (replaces `JokoController`) — A spawnable target. Holds `WhackableConfig config`, `int remainingHits`, `SpriteRenderer sr`, `BoxCollider2D col`, `ParticleSystem stars`. 
  - `Spawn(config)`: set sprite/color/hits, `DOMoveY` pop up (`Ease.OutBack`), schedule auto-hide after stay duration (`DOVirtual.DelayedCall`).
  - `OnHit()`: decrement `remainingHits`. If penalty → `GameManager.RegisterPenalty()` + bomb FX + hide. If tough & hits remain → swap to `hitSprite` + `DOShakePosition` + Hit SFX. If hits == 0 → score via `GameManager.AddScore`, star particles, `DOPunchScale`, Defeat SFX, then `Hide()`.
  - `Hide()`: `DOMoveY` down (`Ease.InBack`), then mark hole free. Kill pending tweens on disable.
- **`InputManager.cs`** — Single tap → single raycast → dispatch to hit `Whackable`. Uses `Pointer.current.press.wasPressedThisFrame`. Ignores input unless `GameState.Playing`.
- **`UIManager.cs`** (refactor) — HUD: `UpdateScore(int)`, `UpdateTimer(float)`, `SetStage(int)`, `SetStrikes(int)`. Game Over panel: `ShowGameOver(bool won, int finalScore)`, Retry (reload `GameplayScene`), Menu (load `MenuScene`).

---

# 3-Stage & 3-Strike Logic (managed inside GameManager)

**On `Start()` (GameplayScene):**
```
var session = GameSession.Instance;
activeStage   = stageConfigs[session.SelectedStage - 1];   // StageConfig SO for chosen stage
difficultyFac = session.DifficultyFactor;                  // 1.0 or hardMultiplier
strikes = 0; score = 0; timeLeft = activeStage.roundDuration; // 30
UIManager.SetStage(session.SelectedStage);
UIManager.SetStrikes(0);
spawnManager.Configure(activeStage, difficultyFac);        // restricts spawnable types to this stage
ChangeState(GameState.Playing);
AudioManager.PlayBGM(Gameplay);
```
**Stage content is enforced purely by `activeStage.allowedTypes`** — SpawnManager can only choose from that set. Stage 1 SO lists {Normal}; Stage 2 {Normal, Penalty}; Stage 3 {Normal, Penalty, Tough}. No `if (stage==...)` branching in spawn code — data-driven.

**Per-frame while Playing:**
```
timeLeft -= Time.deltaTime;
UIManager.UpdateTimer(timeLeft);
if (timeLeft <= 0) { EndGame(won:true); }   // Time Up = win/round complete
```
**Score:** `AddScore(int v)` → `score += v; UIManager.UpdateScore(score);`

**3-strike penalty (called by penalty Whackable):**
```
public void RegisterPenalty() {
    if (currentState != Playing) return;
    strikes++;
    AudioManager.PlaySFX("Strike");
    UIManager.SetStrikes(strikes);
    if (strikes >= 3) EndGame(won:false);   // immediate Game Over
}
```
**End:** `EndGame(bool won)` → `ChangeState(GameOver); spawnManager.StopAll(); UIManager.ShowGameOver(won, score);`
Strikes are **per-round** (reset on Start), matching discrete-stage selection.

---

# Implementation Steps

> Roles: **developer** = writes/edits C# + scene/prefab setup. **explorer** = read-only investigation (not needed further; context already gathered).

### Step 1 — Project configuration
- **Description**: Create Sorting Layers (`Background`, `HoleBack`, `Object`, `HoleFront`) in Tags & Layers. Add `MenuScene` + `GameplayScene` to Build Settings. Confirm DOTween setup (run `Tools → Demigiant → DOTween Utility Panel → Setup` if asmdef references are missing). Set Game view to Portrait 1080x1920; camera Orthographic.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes (with Step 2)

### Step 2 — Persistent core (Bootstrap, GameSession, AudioManager)
- **Description**: Create `GameSession.cs` (data carrier + `Difficulty` enum), refactor `AudioManager.cs` to dictionary-based SFX + `BgmType`, add `Bootstrap.cs`. Both managers `DontDestroyOnLoad`.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes (with Step 1)

### Step 3 — ScriptableObject configs
- **Description**: Implement `WhackableConfig`, `StageConfig`, `DifficultyConfig` SO classes. Author 3 WhackableConfig assets (Normal/Penalty/Tough), 3 StageConfig assets, 1 DifficultyConfig asset with the values in this plan.
- **Assigned role**: developer
- **Dependencies**: None
- **Parallelizable**: Yes

### Step 4 — MenuScene
- **Description**: Build `MenuScene` (uGUI Canvas per UI wireframe), implement `MenuUIManager.cs` writing to `GameSession`, `PLAY` → load `GameplayScene`. Add menu BGM trigger.
- **Assigned role**: developer
- **Dependencies**: Step 2
- **Parallelizable**: No

### Step 5 — GameplayScene greybox & prefabs
- **Description**: Build `GameplayScene`: camera, background, `HoleGrid` with 9 `Hole` prefabs placed per the grid spec (5 back / 4 front), each with HoleBack/SpawnAnchor/HoleFront + sorting layers/orders. Create `Whackable` prefab (SpriteRenderer + BoxCollider2D + ParticleSystem). Greybox colors via primitives.
- **Assigned role**: developer
- **Dependencies**: Step 1, Step 3
- **Parallelizable**: No

### Step 6 — Gameplay scripts
- **Description**: Implement `Whackable.cs`, `Hole.cs`, `SpawnManager.cs`, `InputManager.cs`, `GameManager.cs`, `UIManager.cs` per responsibilities above. Wire references in the scene.
- **Assigned role**: developer
- **Dependencies**: Step 2, Step 3, Step 5
- **Parallelizable**: No

### Step 7 — DOTween motion & FX
- **Description**: Implement pop (`DOMoveY` OutBack), hide (`DOMoveY` InBack), tough-hit `DOShakePosition`, defeat `DOPunchScale`, star particle play, optional BGM `DOFade` crossfade. Ensure all tweens are killed on disable/scene change (no `Update()` translation math).
- **Assigned role**: developer
- **Dependencies**: Step 6
- **Parallelizable**: No

### Step 8 — Audio wiring & polish
- **Description**: Assign placeholder/temporary clips for menu/gameplay BGM and SFX (Pop/Hit/Defeat/Bomb/Strike). Tune difficulty/stage values. Verify sprite-swap readiness (assign hit/damaged sprites where available).
- **Assigned role**: developer
- **Dependencies**: Step 6, Step 7
- **Parallelizable**: No

---

# Verification & Testing

1. **Scene flow**: MenuScene → choose Normal/Stage1 → PLAY → GameplayScene loads with correct stage label; Retry reloads; Menu returns. BGM does not gap (persistent AudioManager).
2. **Stage content enforcement**: Stage 1 spawns only Normal; Stage 2 adds Bombs; Stage 3 spawns all three. Confirm SpawnManager never spawns a type outside `activeStage.allowedTypes`.
3. **Normal object**: 1 tap → score increment + stars + immediate hide.
4. **Penalty/3-strike**: Tapping a bomb increments strike icons; **exactly 3 strikes triggers immediate Game Over** with "GAME OVER" label. 1–2 strikes do not end the round.
5. **Tough object (Stage 3)**: First tap swaps sprite + shakes (no score, not defeated); second tap defeats + scores + stars.
6. **Timer/Win**: Round ends at 0s with "TIME UP!" and correct final score (when <3 strikes).
7. **Difficulty multiplier**: Hard mode is visibly faster (spawn interval, pop speed, stay duration all scaled by `hardMultiplier`); Normal vs Hard differ ONLY by that factor.
8. **Input**: Single raycast per tap registers on the correct collider; no input outside `Playing`; works with mouse (Editor) and touch (device/Simulator).
9. **Pseudo-3D masking**: Objects emerge from oval centers; HoleFront masks their lower half; front row renders over back row.
10. **DOTween hygiene**: No `Update()`-based translation; tweens killed on disable; no warnings/leaks in Console on repeated Retry.
11. **EditMode/Console**: No compile errors or null-ref warnings on scene load (check Unity Console).

---

# Migration Notes (legacy → v2)
- `JokoController.cs` → replaced by `Whackable.cs` (+ centralized `InputManager`, removing per-object polling).
- `GameManager.cs` → rewritten: type-driven stages (not hole-count), 3-strike system (not score penalty), reads `GameSession`, single-stage round.
- `UIManager.cs` → extended with stage label + strike icons + win/lose Game Over.
- `AudioManager.cs` → refactored to dictionary SFX + BgmType; kept DontDestroyOnLoad.
- Single `SampleScene` → split into `MenuScene` + `GameplayScene`.
