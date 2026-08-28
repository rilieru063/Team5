using UnityEngine;

public class DropObject : MonoBehaviour
{
    private float speed;
    private bool canMove = false;

    [Header("‰æ–ÊŠO”»’è")]
    public float destroyY = -6f;
    public void SetMove(float moveSpeed, float delay)
    {
        speed = moveSpeed;
        Invoke(nameof(StartMove), delay);
    }
    void StartMove()
    {
        canMove = true;
    }

    void Update()
    {
        if (!canMove)
            return;

        transform.position += Vector3.down * speed * Time.deltaTime;

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }
}
