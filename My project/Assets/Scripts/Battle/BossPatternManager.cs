using UnityEngine;
using System.Collections;

public class BossPatternManager : MonoBehaviour
{
    //種類決定
    public enum BossType{TutorialBoss, Stage1Boss}

    [Header("ボスの種類")]
    public BossType bossType;

    // Spawner
    [Header("チュートリアルボス")]
    public DropSpawner dropSpawner;

    [Header("1面ボス")]
    public KnifeSpawner knifeSpawner;

    // プレイヤー
    [Header("プレイヤー")]
    public Transform player;


    // 前回のナイフパターン
    private int lastPattern = -1;

    void Start()
    {
        SetBossType();
        StartCoroutine(BossPattern());
    }

    void SetBossType()
    {
        switch (StageManager.CurrentStage)
        {
            case 0:

                bossType = BossType.TutorialBoss;
                break;

            case 1:

                bossType = BossType.Stage1Boss;
                break;
        }
    }


    IEnumerator BossPattern()
    {
        while (true)
        {
            // チュートリアル
            if (bossType == BossType.TutorialBoss)
            {
                yield return StartCoroutine(TutorialBossPattern());
            }

            // 1面
            else if (bossType == BossType.Stage1Boss)
            {
                yield return StartCoroutine(Stage1BossPattern());
            }


            // 次の攻撃まで待つ
            yield return new WaitForSeconds(0.8f);
        }
    }


    // チュートリアルボス
    IEnumerator TutorialBossPattern()
    {
        // 5個落とす
        int count = 1;

        for (int i = 0; i < count; i++)
        {
            if (dropSpawner != null)
            {dropSpawner.SpawnDrop(2f,0f);}

            // 次の落下まで
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 1面ボス
    IEnumerator Stage1BossPattern()
    {
        int pattern;
        do{pattern = Random.Range(0,5);}
        while (pattern == lastPattern);

        lastPattern = pattern;

        // パターン実行

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


        yield return null;
    }

    // Pattern1上から
    void Pattern1()
    {
        if (knifeSpawner == null)
            return;

        knifeSpawner.SpawnKnife(new Vector3(0, 4, 0),-90,0.25f,0.5f);
    }

    // Pattern2 左右から
    void Pattern2()
    {
        if (knifeSpawner == null)
            return;

        knifeSpawner.SpawnKnife(new Vector3(-5, 0, 0), 0,0.5f, 0.5f);

        knifeSpawner.SpawnKnife(new Vector3(5, -4, 0),-180,0.5f,0.5f);
    }

    // Pattern3 全方向から
    void Pattern3()
    {
        if (knifeSpawner == null)
            return;

        SpawnSurroundKnives(8);
    }

    // Pattern4 斜めから
    void Pattern4()
    {
        if (knifeSpawner == null)
            return;

        knifeSpawner.SpawnKnife(new Vector3(-5, 2, 0),-45,0.25f,0.5f);

        knifeSpawner.SpawnKnife( new Vector3(5, 2, 0),-135,0.25f,0.5f);
    }

    // Pattern5 十字から
    void Pattern5()
    {
        if (knifeSpawner == null)
            return;

        knifeSpawner.SpawnKnife(new Vector3(-5, -2, 0),0,0.25f,0.5f );

        knifeSpawner.SpawnKnife(new Vector3(0, 4, 0),-90,0.25f,0.5f);
    }

    void SpawnSurroundKnives(int count)
    {
        if (player == null)
            return;


        float radius = 6f;
        Vector3 targetPosition = player.position;

        for (int i = 0; i < count; i++)
        {
            float angle = 360f / count * i;
            float rad = angle * Mathf.Deg2Rad;


            Vector3 spawnPosition = new Vector3( Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius,0);
            knifeSpawner.SpawnKnifeToTarget( spawnPosition,targetPosition,0.125f,0.5f );
        }
    }
}
