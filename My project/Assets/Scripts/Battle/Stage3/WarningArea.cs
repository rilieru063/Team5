using UnityEngine;

public class WarningArea : MonoBehaviour
{
    [Header("“_–ÅŠÔŠu")]
    [SerializeField] private float blinkInterval = 0.2f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            timer = 0f;

            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }
}
