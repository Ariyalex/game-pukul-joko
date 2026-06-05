# Project Overview
- Game Title: Pukul Joko
- Objective: Normalize sorting order values for all holes from thousands to a manageable range (0-200) while preserving mask isolation.

# Implementation Steps
1. **Re-assign rowSortingOrder in GameplayScene**:
   - Sort holes by Y position (Back row to Front row).
   - Assign new unique values starting from 10 with an increment of 20.
   - Back Row (higher Y): 10, 30, 50, 70, 90.
   - Front Row (lower Y): 110, 130, 150, 170.
   - **Assigned role**: developer
   - **Parallelizable**: No

2. **Verify Visual Sync**:
   - Ensure `Hole.cs` triggers visual updates for these new orders.
   - Confirm characters are visible and masked correctly in the Editor.
   - **Assigned role**: developer
   - **Parallelizable**: No

# Verification & Testing
1. **Inspector Verification**: Confirm all holes have `rowSortingOrder` between 0 and 200.
2. **Depth Consistency**: Ensure objects in front rows are drawn over objects in back rows.
3. **Mask Isolation**: Ensure no character is affected by a neighbor's mask.
