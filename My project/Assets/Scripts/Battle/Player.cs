using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class Borders
    {
        public float xMin, xMax, yMin, yMax;
    }

    [SerializeField] Borders borders;

    [SerializeField, Range(0f, 1f)]
    private float followStrength = 0.175f;

    private float normalFollowStrength;

    private int spiderWebCount = 0;

    [SerializeField, Range(0f, 1f)]
    private float spiderWebFollowStrength = 0.0025f;

    private int damageAmount = 1;

    [Header("É_ÉÅÅ[ÉWñ≥ìG")]
    [SerializeField] private float invincibleTime = 0.5f;

    [SerializeField] private float blinkInterval = 0.1f;

    private bool isInvincible = false;

    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;


    private void Start()
    {
        normalFollowStrength = followStrength;

        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }


    public void SetDamage(int damage)
    {
        damageAmount = damage;
    }


    public void TakeDamage()
    {
        if (isInvincible)
            return;

        LifeManager.Instance.Damage(damageAmount);

        StartCoroutine(InvincibleCoroutine());
    }


    public void TakeDamage(int damage)
    {
        if (isInvincible)
            return;

        LifeManager.Instance.Damage(damage);

        StartCoroutine(InvincibleCoroutine());
    }


    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Clamp(mousePos.x, borders.xMin, borders.xMax);

        mousePos.y = Mathf.Clamp(mousePos.y, borders.yMin, borders.yMax);

        mousePos.z = 0f;

        transform.position = Vector3.Lerp(
            transform.position,
            mousePos,
            followStrength
        );
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }

        if (collision.gameObject.CompareTag("SpiderWeb"))
        {
            spiderWebCount++;

            followStrength = spiderWebFollowStrength;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SpiderWeb"))
        {
            spiderWebCount--;

            if (spiderWebCount > 0)
            {
                return;
            }

            spiderWebCount = 0;
            followStrength = normalFollowStrength;
        }
    }

    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval;
        }

        spriteRenderer.enabled = true;

        isInvincible = false;
    }
}
