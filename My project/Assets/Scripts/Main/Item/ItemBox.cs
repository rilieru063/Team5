using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private int gridX;
    private int gridY;

    public int GridX => gridX;
    public int GridY => gridY;

    public void SetGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
    }

    public void GetItem()
    {
        if (ItemManager.Instance.HasItem())
        {
            Destroy(gameObject);
            return;
        }

        ItemManager.Instance.GetRandomItem();

        Destroy(gameObject);
    }
}