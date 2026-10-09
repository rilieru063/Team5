using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [Header("ミサイル")]
    [SerializeField] private GameObject missilePrefab;

    [Header("生成位置")]
    [SerializeField] private float spawnX = -7f;

    [Header("上半分")]
    [SerializeField] private float upperY = 0f;

    [Header("下半分")]
    [SerializeField] private float lowerY = -4f;

    // 既存の通常ミサイル
    public GameObject SpawnMissile(Transform player, int damage)
    {
        if (missilePrefab == null)
        {
            Debug.LogError("MissilePrefabが設定されていません");
            return null;
        }

        float spawnY = Random.Range(0, 2) == 0
            ? upperY
            : lowerY;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);

        Missile missileScript = missile.GetComponent<Missile>();

        if (missileScript == null)
        {
            Debug.LogError("Missileコンポーネントがありません");
            Destroy(missile);
            return null;
        }

        missileScript.Initialize(player);
        missileScript.SetDamage(damage);

        return missile;
    }

    // 風車型：位置と発射方向を指定するミサイル
    public GameObject SpawnDirectionalMissile(Vector3 spawnPosition,Vector2 direction,int damage)
    {
        if (missilePrefab == null)
        {
            Debug.LogError("MissilePrefabが設定されていません");
            return null;
        }

        GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);

        Missile missileScript = missile.GetComponent<Missile>();

        if (missileScript == null)
        {
            Debug.LogError("Missileコンポーネントがありません");
            Destroy(missile);
            return null;
        }

        missileScript.InitializeDirection(direction);
        missileScript.SetDamage(damage);

        return missile;
    }
}