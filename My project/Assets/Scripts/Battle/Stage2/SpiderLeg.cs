using UnityEngine;
using System.Collections;

public class SpiderLeg : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;
    private bool isMoving = false;

    public bool IsArrived { get; private set; } = false;

    public void Initialize(Vector3 target, float moveSpeed, float length, float width)
    {
        targetPosition = target;
        speed = moveSpeed;

        isMoving = false;
        IsArrived = false;

        transform.localScale = new Vector3(length, width, 1f);

        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 0.5•b‘Ò‚Á‚Ä‚©‚çˆÚ“®ŠJŽn
        StartCoroutine(WaitAndMove(0.5f));
    }

    private IEnumerator WaitAndMove(float delay)
    {
        yield return new WaitForSeconds(delay);

        isMoving = true;
    }

    void Update()
    {
        if (!isMoving)
            return;

        transform.position = Vector3.MoveTowards(transform.position,targetPosition,speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            IsArrived = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}