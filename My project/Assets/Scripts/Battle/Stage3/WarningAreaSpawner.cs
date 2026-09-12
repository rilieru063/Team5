using UnityEngine;

public class WarningAreaSpawner : MonoBehaviour
{
    [Header("警告エリア")]
    [SerializeField] private GameObject warningAreaPrefab;

    [Header("生成範囲")]
    [SerializeField] private float minX = -3f;
    [SerializeField] private float maxX = 3f;

    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 1f;

    [Header("攻撃")]
    [SerializeField] private GameObject bombPrefab;

    [Header("爆弾")]
    [SerializeField] private float bombSpawnY = 7f;


    // 警告エリアを生成
    public GameObject SpawnWarningArea()
    {
        if (warningAreaPrefab == null)
        {
            Debug.LogError("WarningAreaPrefabが設定されていません");
            return null;
        }

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(x, y, 0f);

        GameObject warningArea = Instantiate(warningAreaPrefab, spawnPosition, Quaternion.identity);
        return warningArea;
    }

    public GameObject SpawnBomb(Vector3 targetPosition,Transform player, int damage)
    {
        if (bombPrefab == null)
        {
            Debug.LogError("BombPrefabが設定されていません");
            return null;
        }

        Vector3 spawnPosition = new Vector3(targetPosition.x, bombSpawnY, 0f);

        GameObject bomb = Instantiate(bombPrefab, spawnPosition, Quaternion.identity);

        Bomb bombScript = bomb.GetComponent<Bomb>();

        if (bombScript == null)
        {
            Debug.LogError("BombコンポーネントがBombPrefabにありません");

            return bomb;
        }

        bombScript.SetTargetY(targetPosition.y);
        bombScript.SetPlayer(player);
        bombScript.SetDamage(damage);

        return bomb;
    }
}
