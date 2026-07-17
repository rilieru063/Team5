using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    [Header("生成するKnife")]
    public GameObject knifeObject;

    [Header("ナイフ速度")]
    public float speed = 10f;

    // ナイフを1本生成
    public void SpawnKnife(Vector3 position, float angle)
    {
        GameObject obj = Instantiate(knifeObject,position,Quaternion.Euler(0, 0, angle - 90));

        Knife knife = obj.GetComponent<Knife>();

        if (knife != null)
        {
            knife.SetMove(angle, speed);
        }
        else
        {
            Debug.LogError("KnifeObjectにKnife.csがありません");
        }
    }

    // 指定方向へ複数発射
    public void SpawnKnifeToTarget(Vector3 spawnPosition,Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - spawnPosition;

        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

        SpawnKnife(spawnPosition,angle);
    }
}