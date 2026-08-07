using UnityEngine;
using System.Collections;

public class BossPatternManager : MonoBehaviour
{
    public KnifeSpawner knifeSpawner;
    public Transform player;

    private int lastPattern = -1;

    void Start()
    {
        StartCoroutine(BossPattern());
    }


    IEnumerator BossPattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.8f);

            // 前回と違うパターンをランダム選択
            int pattern;
            do
            {
                pattern = Random.Range(0,5);
            }
            while (pattern == lastPattern);

            lastPattern = pattern;

            switch (pattern)
            {
                case 0:
                    Pattern1();
                    break;

                case 1:
                    Pattern2();
                    break;

                case 2:
                    Pattern3();
                    break;

                case 3:
                    Pattern4();
                    break;

                case 4:
                    Pattern5();
                    break;
            }
        }
    }

    // その1 上から
    void Pattern1()
    {
        knifeSpawner.SpawnKnife(new Vector3(0, 4, 0), -90, 0.25f, 0.5f);
    }

    // その2 左右から
    void Pattern2()
    {
        knifeSpawner.SpawnKnife(new Vector3(-5, 0, 0), 0, 0.5f, 0.5f);
        knifeSpawner.SpawnKnife(new Vector3(5, -4, 0), -180, 0.5f, 0.5f);
    }

    // その3 全方向から
    void Pattern3()
    {
        SpawnSurroundKnives(8);
    }

    // その4 斜めから
    void Pattern4()
    {
        knifeSpawner.SpawnKnife(new Vector3(-5, 2, 0), -45, 0.25f, 0.5f);
        knifeSpawner.SpawnKnife(new Vector3(5, 2, 0), -135, 0.25f, 0.5f);
    }

    // その5 十字から
    void Pattern5()
    {
        knifeSpawner.SpawnKnife(new Vector3(-5, -2, 0), 0, 0.25f, 0.5f);
        knifeSpawner.SpawnKnife(new Vector3(0, 4, 0), -90, 0.25f, 0.5f);
    }

    // 円周上から中心へナイフを飛ばす
    void SpawnSurroundKnives(int count)
    {
        float radius = 6f;

        // 中心
        Vector3 targetPosition = player.position;

        for (int i = 0; i < count; i++)
        {
            // 円周上の角度
            float angle = 360f / count * i;

            float rad = angle * Mathf.Deg2Rad;


            // 出現位置
            Vector3 spawnPosition = new Vector3(Mathf.Cos(rad) * radius,Mathf.Sin(rad) * radius,0);


            // 中心へ発射
            knifeSpawner.SpawnKnifeToTarget(spawnPosition,targetPosition,0.125f, 0.5f);
        }
    }
}