using UnityEngine;
using System.Collections;

public class Knife : MonoBehaviour
{
    private Vector2 direction;
    private float speed;

    private Rigidbody2D rb;

    public float lifeTime = 5f;

    [Header("開始までの待機時間")]
    public float startDelay = 1f;

    private bool canMove = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetMove(float angle, float spd)
    {
        float rad = angle * Mathf.Deg2Rad;// 角度から方向を作成

        direction = new Vector2(Mathf.Cos(rad),Mathf.Sin(rad)).normalized;
        speed = spd;

        StartCoroutine(StartMoving());
    }

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(startDelay);// 指定秒数停止

        canMove = true;
    }

    void FixedUpdate()
    {
        if (!canMove)
            return;

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LifeManager.Instance.Damage(1);
            Destroy(gameObject);
        }
    }
}
