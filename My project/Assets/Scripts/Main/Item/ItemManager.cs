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
    //アイテムA
    private bool knockbackActive = false;
    private int knockbackTurn = 0;
    public bool IsKnockbackActive()
    {
        return knockbackActive;
    }
    //アイテムB
    private bool doubleMoveActive = false;
    private int doubleMoveCount = 0;
    private int moveStep = 0;
    public bool IsDoubleMoveActive()
    {
        return doubleMoveActive;
    }

    void Awake()
    {
        Instance = this;
    }

    public bool HasItem()
    {
        return currentItem != ItemType.None;
    }
    public void ResetItem()
    {
        currentItem = ItemType.None;

        knockbackActive = false;
        knockbackTurn = 0;

        doubleMoveActive = false;
        doubleMoveCount = 0;
        moveStep = 0;

        Debug.Log(currentItem);
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
    public bool DoubleMoveStep()
    {
        moveStep++;

        if (moveStep < 2)
            return false;

        moveStep = 0;
        doubleMoveCount--;

        if (doubleMoveCount <= 0)
            doubleMoveActive = false;

        return true;
    }

    public bool UseItem()
    {
        if (!HasItem())
            return false;

        Debug.Log($"アイテム使用 : {currentItem}");

        switch (currentItem)
        {
            case ItemType.ItemA:
                knockbackActive = true;
                knockbackTurn = 2;
                break;

            case ItemType.ItemB:
                doubleMoveActive = true;
                doubleMoveCount = 3;
                moveStep = 0;
                break;

            case ItemType.ItemC:
                // アイテムCの効果
                break;
        }

        currentItem = ItemType.None;

        return true;
    }
}