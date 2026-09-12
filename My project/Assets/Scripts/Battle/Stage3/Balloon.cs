using UnityEngine;

public class Balloon : MonoBehaviour
{
    private float moveSpeed;

    private int damage = 0;


    public void Initialize(float speed)
    {
        moveSpeed = speed;
    }


    public void SetDamage(int damageAmount)
    {
        damage = damageAmount;
    }


    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (transform.position.y > 7f)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damage <= 0)
            return;

        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}


