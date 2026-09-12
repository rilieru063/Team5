using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BalloonSpawner : MonoBehaviour
{
    [Header("オブジェクト")]
    [SerializeField] private GameObject attackObjectPrefab;
    [SerializeField] private GameObject dummyObjectPrefab;

    [Header("生成位置")]
    [SerializeField] private float spawnY = -6f;

    [Header("レーン設定")]
    [SerializeField] private float minX = -3f;
    [SerializeField] private float maxX = 3f;

    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("ランダム出現設定")]
    [SerializeField] private float minSpawnDelay = 0f;
    [SerializeField] private float maxSpawnDelay = 1.5f;

    public IEnumerator SpawnPattern1(int damage)
    {
        if (attackObjectPrefab == null)
        {
            Debug.LogError("AttackObjectPrefabが設定されていません");
            yield break;
        }

        if (dummyObjectPrefab == null)
        {
            Debug.LogError("DummyObjectPrefabが設定されていません");
            yield break;
        }

        List<bool> objectTypes = new List<bool>();

        for (int i = 0; i < 6; i++)
        {
            objectTypes.Add(true);
        }

        for (int i = 0; i < 2; i++)
        {
            objectTypes.Add(false);
        }

        Shuffle(objectTypes);

        List<int> lanes = new List<int>();

        for (int i = 0; i < 8; i++)
        {
            lanes.Add(i);
        }

        Shuffle(lanes);

        for (int i = 0; i < 8; i++)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);

            yield return new WaitForSeconds(delay);

            int lane = lanes[i];

            SpawnObject(objectTypes[i], lane, damage);
        }
    }

    private void SpawnObject(bool isAttackObject, int lane, int damage)
    {
        GameObject prefab;

        if (isAttackObject)
        {
            prefab = attackObjectPrefab;
        }
        else
        {
            prefab = dummyObjectPrefab;
        }

        float laneSpacing = (maxX - minX) / 7f;

        float spawnX = minX + laneSpacing * lane;

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Balloon balloon = obj.GetComponent<Balloon>();

        if (balloon != null)
        {
            balloon.Initialize(moveSpeed);

            if (isAttackObject)
            {
                balloon.SetDamage(damage);
            }
        }
        else
        {
            Debug.LogWarning("BalloonがPrefabにありません: " + obj.name);
        }
    }

    private void Shuffle(List<bool> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex =
                Random.Range(0, i + 1);

            bool temp = list[i];

            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex =
                Random.Range(0, i + 1);

            int temp = list[i];

            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
