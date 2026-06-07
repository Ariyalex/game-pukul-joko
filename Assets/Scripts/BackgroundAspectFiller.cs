using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundAspectFiller : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        ScaleToFit();
    }

    private void ScaleToFit()
    {
        if (_spriteRenderer.sprite == null || _mainCamera == null) return;

        float viewHeight = _mainCamera.orthographicSize * 2f;
        float viewWidth = viewHeight * _mainCamera.aspect;

        float spriteWidth = _spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = _spriteRenderer.sprite.bounds.size.y;

        if (spriteWidth <= 0 || spriteHeight <= 0) return;

        // Scale = max(viewWidth / spriteWidth, viewHeight / spriteHeight) for Aspect Fill
        float scaleX = viewWidth / spriteWidth;
        float scaleY = viewHeight / spriteHeight;
        float finalScale = Mathf.Max(scaleX, scaleY);

        // Handle parent scaling
        Vector3 localScale = new Vector3(finalScale, finalScale, 1f);
        if (transform.parent != null)
        {
            Vector3 parentScale = transform.parent.lossyScale;
            localScale.x /= (parentScale.x != 0) ? parentScale.x : 1f;
            localScale.y /= (parentScale.y != 0) ? parentScale.y : 1f;
        }

        transform.localScale = localScale;
    }
}
