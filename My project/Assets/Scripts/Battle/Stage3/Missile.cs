using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("発射速度")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("発射前の停止時間")]
    [SerializeField] private float waitTime = 2f;

    private Transform player;

    private bool isLaunched = false;
    private float timer = 0f;

    private int damage;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Update()
    {
        if (!isLaunched)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                isLaunched = true;

                Debug.Log("ミサイル発射");
            }

            return;
        }

        transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        if (transform.position.x > 8f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLaunched)
            return;

        if (collision.CompareTag("Player"))
        {
            Player playerScript = collision.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);

                Debug.Log("Missileがプレイヤーに命中");
            }
        }
    }


    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }
}

