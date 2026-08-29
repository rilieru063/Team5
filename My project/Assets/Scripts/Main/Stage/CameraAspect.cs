using UnityEngine;

public class CameraAspect : MonoBehaviour
{
    public float defaultWidth = 1920f;
    public float defaultHeight = 1080f;

    void Start()
    {
        Camera mainCamera = Camera.main;

        float targetAspect = defaultWidth / defaultHeight;
        float windowAspect = (float)Screen.width / Screen.height;

        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // ‰æ–Ê‚ªc’·
            mainCamera.rect = new Rect(
                0,
                (1.0f - scaleHeight) / 2.0f,
                1.0f,
                scaleHeight
            );
        }
        else
        {
            // ‰æ–Ê‚ª‰¡’·
            float scaleWidth = 1.0f / scaleHeight;

            mainCamera.rect = new Rect(
                (1.0f - scaleWidth) / 2.0f,
                0,
                scaleWidth,
                1.0f
            );
        }
    }
}