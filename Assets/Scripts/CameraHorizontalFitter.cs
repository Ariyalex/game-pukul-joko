using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraHorizontalFitter : MonoBehaviour
{
    public float targetWidth = 11.0f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        float aspect = _camera.aspect;
        if (aspect <= 0) return;

        // Formula: orthographicSize = (targetWidth / 2) / aspect
        float size = (targetWidth / 2f) / aspect;
        
        // Use Mathf.Max to ensure it doesn't get too small if the screen is very wide.
        // We use a small positive value to prevent orthographicSize from being <= 0.
        _camera.orthographicSize = Mathf.Max(size, 0.1f);
    }
}
