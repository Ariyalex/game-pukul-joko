using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centralized tap input. On a single pointer press (touch or mouse, unified by the New
/// Input System's <see cref="Pointer"/>), performs ONE Physics2D.Raycast and forwards the
/// hit to a <see cref="Whackable"/>. Only active during GameState.Playing.
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera worldCamera;

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;
    }

    private void Update()
    {
        // Gate input to active play.
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameState.Playing)
            return;

        var pointer = Pointer.current;
        if (pointer == null || !pointer.press.wasPressedThisFrame) return;

        if (worldCamera == null) worldCamera = Camera.main;
        if (worldCamera == null) return;

        Vector2 screenPos = pointer.position.ReadValue();
        Vector2 worldPos = worldCamera.ScreenToWorldPoint(screenPos);

        // Single raycast; zero direction = point overlap test at worldPos.
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<Whackable>(out var whackable))
            whackable.OnHit();
    }
}
