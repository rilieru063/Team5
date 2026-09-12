using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("落下速度")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("着弾時のダメージ範囲")]
    [SerializeField] private float damageRadius = 1.5f;

    private float targetY;
    private bool hasTarget = false;

    private Transform player;

    private bool hasLanded = false;

    private int damage;

    public void SetTargetY(float y)
    {
        targetY = y;
        hasTarget = true;
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Update()
    {
        if (!hasTarget)
            return;

        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (transform.position.y <= targetY)
        {
            transform.position = new Vector3(transform.position.x, targetY, transform.position.z);

            Land();
        }
    }

    private void Land()
    {
        if (hasLanded)
            return;

        hasLanded = true;

        Debug.Log("Bomb 着弾");

        CheckDamage();

        Destroy(gameObject);
    }


    private void CheckDamage()
    {
        if (player == null)
        {
            Debug.LogWarning("Playerが設定されていません");
            return;
        }


        float distance = Vector2.Distance(transform.position, player.position);


        Debug.Log( "爆弾とプレイヤーの距離: " + distance );

        if (distance <= damageRadius)
        {
            Player playerScript = player.GetComponent<Player>();

            if (playerScript != null)
            {
                playerScript.TakeDamage(damage);

                Debug.Log("爆弾がプレイヤーに命中");
            }
        }
    }

    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }

}

