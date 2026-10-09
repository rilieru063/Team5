using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("発射速度")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("発射前の停止時間")]
    [SerializeField] private float waitTime = 2f;

    private Transform player;
    private Vector2 moveDirection = Vector2.right;

    private bool isLaunched = false;
    private float timer = 0f;
    private int damage;

    // 通常ミサイル：右方向へ発射
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        moveDirection = Vector2.right;
        isLaunched = false;
        timer = 0f;
    }

    // 風車型：指定された方向へ発射
    public void InitializeDirection(Vector2 direction)
    {
        player = null;
        moveDirection = direction.normalized;

        // 飛ぶ方向にミサイルを向ける
        float angle = Mathf.Atan2(moveDirection.y,moveDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        isLaunched = false;
        timer = 0f;
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

        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);

        // 画面外に出たら削除
        if (transform.position.x > 10f ||
            transform.position.x < -10f ||
            transform.position.y > 10f ||
            transform.position.y < -10f)
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
