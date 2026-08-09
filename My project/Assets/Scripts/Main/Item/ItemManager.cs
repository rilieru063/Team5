using UnityEngine;

public enum ItemType
{
    None,
    ItemA,
    ItemB,
    ItemC
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private ItemType currentItem = ItemType.None;
    private bool knockbackActive = false;
    private int knockbackTurn = 0;

    public bool KBA => knockbackActive;
    public int KBT => knockbackTurn;

    void Awake()
    {
        Instance = this;
    }

    public bool HasItem()
    {
        return currentItem != ItemType.None;
    }

    public void GetRandomItem()
    {
        if (HasItem())
            return;

        currentItem = (ItemType)Random.Range(1, 4);
        Debug.Log($"アイテム取得 : {currentItem}");
    }

    public void EnemyTurn()
    {
        if (!knockbackActive)
            return;

        knockbackTurn--;

        if (knockbackTurn <= 0)
        {
            knockbackActive = false;
        }
    }

    public void UseItem()
    {
        if (!HasItem())
            return;

        Debug.Log($"アイテム使用 : {currentItem}");

        switch (currentItem)
        {
            case ItemType.ItemA:
                knockbackActive = true;
                knockbackTurn = 2;
                break;

            case ItemType.ItemB:
                // アイテムBの効果
                break;

            case ItemType.ItemC:
                // アイテムCの効果
                break;
        }

        currentItem = ItemType.None;
    }
}