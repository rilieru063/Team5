using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField] private float horizontalSpeed = 6f;
    [SerializeField] private float verticalSpeed = 3f;

    [Header("上下のバウンド範囲")]
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [Header("プレイヤーへの当たり判定")]
    [SerializeField] private float damageRadius = 0.5f;

    private Transform player;

    private float verticalDirection = -1f;

    private bool isTouchingPlayer = false;

    private int damage;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;

        verticalDirection = -1f;
    }


    private void Update()
    {
        float moveX = horizontalSpeed;

        float moveY = verticalSpeed * verticalDirection;

        Vector3 movement = new Vector3(moveX, moveY, 0f);

        transform.position += movement * Time.deltaTime;

        CheckBounce();

        CheckPlayerHit();

        if (transform.position.x > 9.5f)
        {
            Destroy(gameObject);
        }
    }


    private void CheckBounce()
    {
        if (transform.position.y <= minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);

            verticalDirection = 1f;
        }

        if (transform.position.y >= maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);

            verticalDirection = -1f;
        }
    }


    private void CheckPlayerHit()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= damageRadius)
        {
            if (!isTouchingPlayer)
            {
                Player playerScript = player.GetComponent<Player>();

                if (playerScript != null)
                {
                    playerScript.TakeDamage(damage);

                    Debug.Log("Ballがプレイヤーに命中");

                    isTouchingPlayer = true;
                }
            }
        }
        else
        {
            // プレイヤーから離れた
            isTouchingPlayer = false;
        }
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }
}