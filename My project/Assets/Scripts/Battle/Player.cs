using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class Borders
    {
        public float xMin, xMax, yMin, yMax;
    }
    [SerializeField] Borders borders;
    [SerializeField, Range(0f, 1f)] private float followStrength;//í«è]ÇÃíxÇÍ(0Ç…ãﬂÇ¢Ç∆í«è]Ç™íxÇÍÇÈ)

    private void Start()
    {

    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.x = Mathf.Clamp(mousePos.x, borders.xMin, borders.xMax);
        mousePos.y = Mathf.Clamp(mousePos.y, borders.yMin, borders.yMax);
        mousePos.z = 0f;
        transform.position = Vector3.Lerp(transform.position, mousePos, followStrength);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LifeManager.Instance.Damage(1);
        }
    }
}
