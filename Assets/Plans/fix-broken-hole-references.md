# Project Overview
- Game Title: Pukul Joko
- High-Level Concept: Whack-a-Mole game with 3 stages and persistent difficulty.
- Fix: Repairing broken references in the SpawnManager.

# Bug Analysis
- **Problem**: Only one hole is spawning objects.
- **Cause**: The `SpawnManager` holds a list of `Hole` component references. When the user deleted and re-copied the holes in the hierarchy, 8 out of 9 references in the `SpawnManager`'s inspector became "Missing". Only the reference to the original `Hole_Front_0` (which was likely kept and copied) remained valid at its original array index.
- **Evidence**: Inspection showed `SpawnManager.holes` has 9 elements, where index 5 is `Hole_Front_0` and all others are `MISSING/NULL`.

# Implementation Steps
1. **Update Hole.cs for Editor Sync**:
   - Add `OnValidate()` method to call `ApplySortingOrders()`.
   - Update `ApplySortingOrders()` to synchronize the `Whackable` object's sorting order even in Edit Mode.
   - Widen the `SpriteMask` range to include potential children objects (Order +2 to +5).
   - **Assigned role**: developer
   - **Parallelizable**: No

2. **Fix Broken References & Visuals**:
   - Execute a script to re-link all `Hole` objects to the `SpawnManager` and trigger a refresh on all holes.
   - **Assigned role**: developer
   - **Parallelizable**: No

3. **Verify Scene Persistence**:
   - Save the `GameplayScene` to ensure the new references and visual fixes are stored.
   - **Assigned role**: developer
   - **Parallelizable**: No

# Verification & Testing
1. **Runtime Spawn Test**: Run the game and observe if objects appear randomly from all 9 holes across the grid, not just one.
2. **Inspector Check**: Verify that the `SpawnManager` component no longer shows any "Missing" references.
