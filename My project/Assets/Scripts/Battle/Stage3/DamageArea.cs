using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [Header("É_ÉÅÅ[ÉW")]
    [SerializeField] private int damage = 10;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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

