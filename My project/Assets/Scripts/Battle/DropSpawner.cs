using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [Header("生成する落下オブジェクト")]
    public GameObject dropObject;

    [Header("落下速度")]
    public float speed = 5f;

    [Header("落下範囲")]
    public float minX = -3f;
    public float maxX = 3f;

    [Header("出現Y座標")]
    public float spawnY = 6f;

// 落下オブジェクトを生成
    public void SpawnDrop(float size, float delay)
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX,spawnY,0f);

        GameObject obj = Instantiate(dropObject,spawnPosition,Quaternion.identity);
        obj.transform.localScale = Vector3.one * size;
        DropObject drop = obj.GetComponent<DropObject>();

        if (drop != null)
        {
            drop.SetMove(speed, delay);
        }
        else
        {
            Debug.LogError("DropObjectにDropObject.csがありません");
        }
    }
}
