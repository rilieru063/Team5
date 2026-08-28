using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossPatternManager : MonoBehaviour
{
    //種類決定
    public enum BossType{TutorialBoss, Stage1Boss, Stage2Boss}

    [Header("ボスの種類")]
    public BossType bossType;

    // Spawner
    [Header("チュートリアルボス")]
    public DropSpawner dropSpawner;

    [Header("1面ボス")]
    public KnifeSpawner knifeSpawner;

    [Header("2面ボス")]
    public SpiderLegSpawner spiderLegSpawner;

    [Header("蜘蛛の巣")]
    public GameObject spiderWebPrefab;

    // Player
    [Header("プレイヤー")]
    public Transform player;

    // PlayerDamage
    [Header("プレイヤーへのダメージ")]
    [SerializeField] private int tutorialBossDamage = 20;
    [SerializeField] private int stage1BossDamage = 2;
    [SerializeField] private int stage2BossDamage = 3;

    // 前回のナイフパターン
    private int lastPattern = -1;

    // Stage2で前回選択したパターン
    private int lastStage2Pattern = -1;

    // ボスが倒されたか
    private bool bossDefeated = false;

    void Start()
    {
        SetBossType();
        SetPlayerDamage();

        StartCoroutine(BossPattern());
        Debug.Log(Life.Instance.lifepoint);
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

            case 2:
                bossType = BossType.Stage2Boss;
                break;
        }
    }

    void SetPlayerDamage()
    {
        if (player == null)
        {
            Debug.LogError("Playerが設定されていません");
            return;
        }

        Player playerScript = player.GetComponent<Player>();

        if (playerScript == null)
        {
            Debug.LogError("Playerコンポーネントが見つかりません");
            return;
        }

        switch (bossType)
        {
            case BossType.TutorialBoss:
                playerScript.SetDamage(tutorialBossDamage);
                break;

            case BossType.Stage1Boss:
                playerScript.SetDamage(stage1BossDamage);
                break;

            case BossType.Stage2Boss:
                playerScript.SetDamage(stage2BossDamage);
                break;
        }
    }

    IEnumerator BossPattern()
    {
        while (!bossDefeated)
        {
            // Tutorial
            if (bossType == BossType.TutorialBoss)
            {
                yield return StartCoroutine(TutorialBossPattern());
            }

            // Stage1
            else if (bossType == BossType.Stage1Boss)
            {
                yield return StartCoroutine(Stage1BossPattern());
            }

            else if (bossType == BossType.Stage2Boss)
            {
                yield return StartCoroutine(Stage2BossPattern());
            }

            //Life消費処理
            if (!Tutorial.Instance.onTutorial)
            {
                if (Life.Instance != null)
                {
                    Life.Instance.lifeminus(1);
                }
                else
                {
                    Debug.LogError("Life.Instance が nullです");
                }
            }

            if (Life.Instance != null && Life.Instance.lifepoint <= 0)
            {
                BossDefeated();

                yield break;
            }


            // 次の攻撃まで待つ
            yield return new WaitForSeconds(0.8f);
        }
    }


    // チュートリアルボス
    IEnumerator TutorialBossPattern()
    {
        if (!Tutorial.Instance.onTutorial)
        {
            // 5個落とす
            int count = 1;

            for (int i = 0; i < count; i++)
            {
                if (dropSpawner != null)
                { dropSpawner.SpawnDrop(3.5f,0f); }

                // 次の落下まで
                yield return new WaitForSeconds(0.05f);
            }
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

    // 2面ボス
    IEnumerator Stage2BossPattern()
    {
        int pattern;
        do{pattern = Random.Range(0, 3);}
        while (pattern == lastStage2Pattern);

        lastStage2Pattern = pattern;

        switch (pattern)
        {
            case 0:
                yield return StartCoroutine(Stage2Pattern1());
                break;

            case 1:
                yield return StartCoroutine(Stage2Pattern2());
                break;

            case 2:
                yield return StartCoroutine(Stage2Pattern3());
                break;
        }
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

    // 2面ボスパターン1
    IEnumerator Stage2Pattern1()
    {
        if (spiderWebPrefab == null)
        {
            Debug.LogError("SpiderWebPrefabが設定されていません");
            yield break;
        }

        Vector3 webPosition = player.position;

        GameObject web = Instantiate(spiderWebPrefab,webPosition,Quaternion.identity);

        SpiderWeb spiderWeb =web.GetComponent<SpiderWeb>();

        if (spiderWeb == null)
        {
            Debug.LogError("SpiderWebコンポーネントが見つかりません");

            Destroy(web);
            yield break;
        }

        spiderWeb.Initialize(0.5f,3f,2f,0.25f);

        yield return new WaitForSeconds(0.5f);
    }
    // 2面パターン2
    IEnumerator Stage2Pattern2()
    {
        if (spiderLegSpawner == null)
        {
            Debug.LogError("SpiderLegSpawnerが設定されていません");
            yield break;
        }

        float leftStopX = -3.75f;
        float rightStopX = 3.75f;

        float spawnX = 7f;

        float[] yPositions ={ 0f,-2f,-4f};

        GameObject[] legs = new GameObject[6];

        for (int i = 0; i < 3; i++)
        {
            legs[i] = spiderLegSpawner.SpawnLeg(new Vector3(-spawnX, yPositions[i], 0f),new Vector3(leftStopX, yPositions[i], 0f));
        }

        for (int i = 0; i < 3; i++)
        {
            legs[i + 3] = spiderLegSpawner.SpawnLeg(new Vector3(spawnX, yPositions[i], 0f),new Vector3(rightStopX, yPositions[i], 0f));
        }

        bool allArrived = false;

        while (!allArrived)
        {
            allArrived = true;

            for (int i = 0; i < legs.Length; i++)
            {
                if (legs[i] == null)
                    continue;

                SpiderLeg leg = legs[i].GetComponent<SpiderLeg>();

                if (leg != null && !leg.IsArrived)
                {
                    allArrived = false;
                    break;
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < legs.Length; i++)
        {
            if (legs[i] != null)
            {
                Destroy(legs[i]);
            }
        }
    }

    // 2面ステージパターン3
    IEnumerator Stage2Pattern3()
    {
        if (spiderLegSpawner == null)
        {
            Debug.LogError("SpiderLegSpawnerが設定されていません");
            yield break;
        }

        float spawnRadius = 7f;

        Vector3 safePosition = new Vector3(0f, -2f, 0f);

        float safeRadius = 4f;

        GameObject[] legs = new GameObject[6];

        for (int i = 0; i < 6; i++)
        {
            float angle = 60f * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 spawnPosition = new Vector3(Mathf.Cos(rad) * spawnRadius,Mathf.Sin(rad) * spawnRadius,0f);

            legs[i] = spiderLegSpawner.SpawnLegToSafePosition(spawnPosition,safePosition,safeRadius);
        }

        bool allArrived = false;

        while (!allArrived)
        {
            allArrived = true;

            for (int i = 0; i < legs.Length; i++)
            {
                if (legs[i] == null)
                    continue;

                SpiderLeg leg =
                    legs[i].GetComponent<SpiderLeg>();

                if (leg != null && !leg.IsArrived)
                {
                    allArrived = false;
                    break;
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < legs.Length; i++)
        {
            if (legs[i] != null)
            {
                Destroy(legs[i]);
            }
        }
    }

    void BossDefeated()
    {
        bossDefeated = true;
        StopAllCoroutines();

        SceneManager.LoadScene("gameclear");
    }
}
