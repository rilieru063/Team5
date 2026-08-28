using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class Borders
    {
        public float xMin, xMax, yMin, yMax;
    }

    [SerializeField] Borders borders;

    [SerializeField, Range(0f, 1f)]
    private float followStrength;

    private float normalFollowStrength;

    private int spiderWebCount = 0;

    [SerializeField, Range(0f, 1f)]
    private float spiderWebFollowStrength = 0.0025f;

    private int damageAmount = 1;


    private void Start()
    {
        normalFollowStrength = followStrength;
    }


    public void SetDamage(int damage)
    {
        damageAmount = damage;
    }


    private void Update()
    {
        Vector3 mousePos =Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Clamp(mousePos.x,borders.xMin,borders.xMax);

        mousePos.y = Mathf.Clamp(mousePos.y,borders.yMin,borders.yMax);

        mousePos.z = 0f;

        transform.position = Vector3.Lerp(transform.position,mousePos,followStrength);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LifeManager.Instance.Damage(damageAmount);
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
}