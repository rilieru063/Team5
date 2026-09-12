using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [Header("ミサイル")]
    [SerializeField] private GameObject missilePrefab;

    [Header("生成位置")]
    [SerializeField] private float spawnX = -7f;

    [Header("上半分")]
    [SerializeField] private float upperY = 2f;

    [Header("下半分")]
    [SerializeField] private float lowerY = -2f;


    public GameObject SpawnMissile(Transform player,int damage)
    {
        if (missilePrefab == null)
        {
            Debug.LogError("MissilePrefabが設定されていません");
            return null;
        }

        bool isUpper = Random.Range(0, 2) == 0;


        float spawnY;

        if (isUpper)
        {
            spawnY = upperY;
        }
        else
        {
            spawnY = lowerY;
        }


        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);


        GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);


        Missile missileScript = missile.GetComponent<Missile>();


        if (missileScript == null)
        {
            Debug.LogError("MissileコンポーネントがMissilePrefabにありません");

            return missile;
        }

        missileScript.Initialize(player);
        missileScript.SetDamage(damage);

        return missile;

    }
}
