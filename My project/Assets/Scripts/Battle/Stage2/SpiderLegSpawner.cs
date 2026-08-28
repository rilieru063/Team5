using UnityEngine;

public class SpiderLegSpawner : MonoBehaviour
{
    [Header("蜘蛛の足Prefab")]
    [SerializeField] private GameObject spiderLegPrefab;

    [Header("移動速度")]
    [SerializeField] private float speed = 5f;

    [Header("蜘蛛の足のサイズ")]
    [SerializeField] private float legLength = 0.3f;
    [SerializeField] private float legWidth = 0.3f;

    public GameObject SpawnLeg(Vector3 spawnPosition, Vector3 targetPosition)
    {
        if (spiderLegPrefab == null)
        {
            Debug.LogError("SpiderLegPrefabが設定されていません");
            return null;
        }

        GameObject leg = Instantiate(spiderLegPrefab, spawnPosition, Quaternion.identity);

        SpiderLeg spiderLeg = leg.GetComponent<SpiderLeg>();

        if (spiderLeg == null)
        {
            Debug.LogError("SpiderLegコンポーネントが見つかりません");

            Destroy(leg);
            return null;
        }

        spiderLeg.Initialize(targetPosition, speed, legLength, legWidth);

        return leg;
    }


    public GameObject SpawnLegToSafePosition(Vector3 spawnPosition, Vector3 safePosition, float safeRadius)
    {
        if (spiderLegPrefab == null)
        {
            Debug.LogError("SpiderLegPrefabが設定されていません");
            return null;
        }

        GameObject leg = Instantiate(spiderLegPrefab, spawnPosition, Quaternion.identity);

        SpiderLeg spiderLeg = leg.GetComponent<SpiderLeg>();

        if (spiderLeg == null)
        {
            Debug.LogError("SpiderLegコンポーネントが見つかりません");

            Destroy(leg);
            return null;
        }

        Vector3 direction = (safePosition - spawnPosition).normalized;

        Vector3 targetPosition = safePosition - direction * safeRadius;

        spiderLeg.Initialize(targetPosition, speed, legLength, legWidth);

        return leg;
    }
}