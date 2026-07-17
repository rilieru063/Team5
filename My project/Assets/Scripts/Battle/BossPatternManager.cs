using UnityEngine;
using System.Collections;

public class BossPatternManager : MonoBehaviour
{
    public KnifeSpawner knifeSpawner;
    public Transform player;


    void Start()
    {
        StartCoroutine(BossPattern());
    }


    IEnumerator BossPattern()
    {
        while (true)
        {
            // その1 上から
            yield return new WaitForSeconds(1f);
            knifeSpawner.SpawnKnife(new Vector3(0, 5, 0),-90);

            // その2 左右から
            yield return new WaitForSeconds(2f);
            knifeSpawner.SpawnKnife(new Vector3(-5, 0, 0),0);
            knifeSpawner.SpawnKnife(new Vector3(5, -4, 0),180);

            // その3 全方向
            yield return new WaitForSeconds(3f);
            SpawnSurroundKnives(8);

            // その4 斜めから
            yield return new WaitForSeconds(2f);
            knifeSpawner.SpawnKnife(new Vector3(-5, 2, 0), -45);
            knifeSpawner.SpawnKnife(new Vector3(5, 2, 0), -135);

            // 次のループまで待機
            yield return new WaitForSeconds(5f);
        }
    }

    // 円周上から中心へナイフを飛ばす
    void SpawnSurroundKnives(int count)
    {
        float radius = 6f;

        // 集まる中心
        Vector3 targetPosition = player.position;

        for (int i = 0; i < count; i++)
        {
            // 円周上の角度
            float angle = 360f / count * i;

            float rad = angle * Mathf.Deg2Rad;


            // 出現位置
            Vector3 spawnPosition = new Vector3(Mathf.Cos(rad) * radius,Mathf.Sin(rad) * radius,0);


            // 中心へ発射
            knifeSpawner.SpawnKnifeToTarget(spawnPosition,targetPosition);
        }
    }
}