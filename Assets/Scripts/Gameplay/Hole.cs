using UnityEngine;

/// <summary>
/// Represents one oval hole in the pseudo-3D grid. Owns a single pooled <see cref="Whackable"/>
/// that pops from <see cref="spawnAnchor"/>. The HoleBack/HoleFront renderers (assigned in the
/// prefab) sit on their respective sorting layers to mask the object as it emerges.
/// </summary>
public class Hole : MonoBehaviour
{
    [Header("Visual parts")]
    [SerializeField] private SpriteRenderer holeBack;   // sorting layer: HoleBack
    [SerializeField] private SpriteRenderer rimBack;    // sorting layer: HoleBack
    [SerializeField] private SpriteRenderer rimFront;   // sorting layer: HoleFront
    [SerializeField] private SpriteMask[] spriteMasks;  // for isolated masking (covers both Circle and Upper)
    [SerializeField] private Transform spawnAnchor;     // oval center
    [SerializeField] private Whackable whackable;       // pooled occupant

    [Header("Pop geometry")]
    [Tooltip("Local Y offset (relative to anchor) the object rises to when popped up.")]
    [SerializeField] private float popHeight = 1.1f;
    [Tooltip("How far below the anchor the object hides (should be hidden behind HoleFront).")]
    [SerializeField] private float hideDepth = 0.6f;

    [Header("Depth ordering")]
    [Tooltip("Base sorting order. Manually set this to unique values to isolate holes.")]
    [SerializeField] private int rowSortingOrder = 0;

    public bool IsOccupied { get; private set; }

    private void Awake()
    {
        ApplySortingOrders();

        if (whackable != null && spawnAnchor != null)
        {
            // Object rest positions are expressed in the whackable's local space
            float anchorLocalY = whackable.transform.localPosition.y;
            float hiddenY = anchorLocalY - hideDepth;
            float shownY = anchorLocalY + popHeight;
            
            // Whackable gets order +2 (sandwiched between back parts and front parts)
            whackable.Initialise(this, hiddenY, shownY, rowSortingOrder + 2);
        }
    }

    private void ApplySortingOrders()
    {
        if (holeBack != null)
        {
            holeBack.sortingLayerName = "HoleBack";
            holeBack.sortingOrder = rowSortingOrder;
        }
        if (rimBack != null)
        {
            rimBack.sortingLayerName = "HoleBack";
            rimBack.sortingOrder = rowSortingOrder + 1;
        }
        if (rimFront != null)
        {
            rimFront.sortingLayerName = "HoleFront";
            rimFront.sortingOrder = rowSortingOrder + 10; 
        }
        
        if (spriteMasks != null)
        {
            int objectLayerID = SortingLayer.NameToID("Object");
            foreach (var mask in spriteMasks)
            {
                if (mask == null) continue;
                mask.isCustomRangeActive = true;
                mask.frontSortingLayerID = objectLayerID;
                mask.frontSortingOrder = rowSortingOrder + 9;
                mask.backSortingLayerID = objectLayerID;
                mask.backSortingOrder = rowSortingOrder;
            }
        }
    }

    private void OnValidate()
    {
        ApplySortingOrders();
    }

    /// <summary>True if this hole can accept a new spawn right now.</summary>
    public bool CanSpawn => !IsOccupied && whackable != null;

    /// <summary>Spawns the given archetype from this hole.</summary>
    public void Spawn(WhackableConfig config, float popDuration, float stayDuration)
    {
        if (!CanSpawn) return;
        IsOccupied = true;
        whackable.Spawn(config, popDuration, stayDuration);
    }

    /// <summary>Callback from the Whackable once it has fully retracted.</summary>
    public void OnWhackableHidden() => IsOccupied = false;

    /// <summary>Forces the hole empty (used when the round stops).</summary>
    public void ForceReset()
    {
        if (whackable != null) whackable.KillTweens();
        IsOccupied = false;
    }
}
