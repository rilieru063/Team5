using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BossPatternManager : MonoBehaviour
{
    //種類決定
    public enum BossType{TutorialBoss, Stage1Boss, Stage2Boss, Stage3Boss}

    [Header("ボスの種類")]
    public BossType bossType;

    // Spawner
    [Header("チュートリアルボス")]
    public DropSpawner dropSpawner;

    [Header("1面ボス")]
    public KnifeSpawner knifeSpawner;

    [Header("2面ボス")]
    public SpiderLegSpawner spiderLegSpawner;
    public GameObject spiderWebPrefab;

    [Header("3面ボス")]
    public BalloonSpawner balloonSpawner;
    public WarningAreaSpawner warningAreaSpawner;
    public BallSpawner ballSpawner;
    public MissileSpawner missileSpawner;

    [Header("Boss表示")]
    [SerializeField] private EnemyDisplayManager enemyDisplayManager;

    // Player
    [Header("プレイヤー")]
    public Transform player;

    // PlayerDamage
    [Header("プレイヤーへのダメージ")]
    [SerializeField] private int tutorialBossDamage = 15;
    [SerializeField] private int stage1BossDamage = 3;
    [SerializeField] private int stage2BossDamage = 9;

    [Header("2面ボス攻撃後の残留時間")]
    [SerializeField] private float stage2Pattern2StayTime = 1.0f;
    [SerializeField] private float stage2Pattern3StayTime = 1.0f;

    [Header("3面ボス攻撃ダメージ")]
    [SerializeField] private int stage3BalloonDamage = 5;
    [SerializeField] private int stage3BombDamage = 30;
    [SerializeField] private int stage3BallDamage = 5;
    [SerializeField] private int stage3MissileDamage = 25;

    [Header("次の攻撃までのクールダウン")]
    [SerializeField] private float tutorialCooldown = 1.0f;
    [SerializeField] private float stage1Cooldown = 0.8f;
    [SerializeField] private float stage2Cooldown = 1.0f;
    [SerializeField] private float stage3Cooldown = 1.0f;

    // 前回のナイフパターン
    private int lastPattern = -1;

    // Stage2で前回選択したパターン
    private int lastStage2Pattern = -1;

    // Stage3で前回選択したパターン
    private int lastStage3Pattern = -1;

    // ボスが倒されたか
    private bool bossDefeated = false;

    // ボスの行動コルーチン
    private Coroutine bossPatternCoroutine;

    // 現在盤面に残っているDamageArea
    private GameObject currentDamageArea;

    void Start()
    {
        SetBossType();
        SetPlayerDamage();

        bossPatternCoroutine = StartCoroutine(BossPattern());
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

            case 3:
                bossType = BossType.Stage3Boss;
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

            case BossType.Stage3Boss:
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

                yield return new WaitForSeconds(tutorialCooldown);
            }

            // Stage1
            else if (bossType == BossType.Stage1Boss)
            {
                yield return StartCoroutine(Stage1BossPattern());

                yield return new WaitForSeconds(stage1Cooldown);
            }

            // Stage2
            else if (bossType == BossType.Stage2Boss)
            {
                yield return StartCoroutine(Stage2BossPattern());

                yield return new WaitForSeconds(stage2Cooldown);
            }

            // Stage3
            else if (bossType == BossType.Stage3Boss)
            {
                yield return StartCoroutine(Stage3BossPattern());

                yield return new WaitForSeconds(stage3Cooldown);
            }

            if (!Tutorial.Instance.onTutorial)
            {
                if (Life.Instance != null)
                {
                    Life.Instance.lifeminus(1);
                }
            }

            if (Life.Instance != null && Life.Instance.lifepoint <= 0)
            {
                StartCoroutine(BossDefeated());

                yield break;
            }

            //if (DebugMode.Instance.win == true)
            //{
            //    DebugMode.Instance.win = false;
            //    StartCoroutine(BossDefeated());

            //    yield break;
            //}
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
                { dropSpawner.SpawnDrop(0.5f,0f); }

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

    // 3面ボス
    IEnumerator Stage3BossPattern()
    {
        int pattern;

        do{pattern = Random.Range(0, 5);}
        while (pattern == lastStage3Pattern);

        lastStage3Pattern = pattern;


        switch (pattern)
        {
            case 0:
                yield return StartCoroutine(Stage3Pattern1());
                break;

            case 1:
                 yield return StartCoroutine(Stage3Pattern2());
                break;

            case 2:
                yield return StartCoroutine(Stage3Pattern3());
                break;

            case 3:
                yield return StartCoroutine(Stage3Pattern4());
                break;

            case 4:
                yield return StartCoroutine(Stage3Pattern5());
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

        yield return new WaitForSeconds(stage2Pattern2StayTime);

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

        yield return new WaitForSeconds(stage2Pattern3StayTime);

        for (int i = 0; i < legs.Length; i++)
        {
            if (legs[i] != null)
            {
                Destroy(legs[i]);
            }
        }
    }

    // 3面パターン1
    IEnumerator Stage3Pattern1()
    {
        if (balloonSpawner == null)
        {
            Debug.LogError("BalloonSpawnerが設定されていません");
            yield break;
        }

        yield return StartCoroutine(balloonSpawner.SpawnPattern1(stage3BalloonDamage));
    }

    // 3面パターン2
    IEnumerator Stage3Pattern2()
    {
        if (warningAreaSpawner == null)
        {
            Debug.LogError("WarningAreaSpawnerが設定されていません");
            yield break;
        }

        if (currentDamageArea != null)
        {
            Destroy(currentDamageArea);
            currentDamageArea = null;
        }

        GameObject warningArea = warningAreaSpawner.SpawnWarningArea();

        if (warningArea == null)
        {
            Debug.LogError("警告エリアの生成に失敗しました");
            yield break;
        }

        yield return new WaitForSeconds(2f);

        Vector3 damagePosition = warningArea.transform.position;

        GameObject bomb = warningAreaSpawner.SpawnBomb( damagePosition, player, stage3BombDamage);

        if (bomb == null)
        {
            Destroy(warningArea);
            yield break;
        }

        while (bomb != null)
        {
            yield return null;
        }

        Destroy(warningArea);

        currentDamageArea = warningAreaSpawner.SpawnDamageArea(damagePosition);

        if (currentDamageArea == null)
        {
            yield break;
        }

    }

    // 3面パターン3
    IEnumerator Stage3Pattern3()
    {
        if (ballSpawner == null)
        {
            Debug.LogError("BallSpawnerが設定されていません");
            yield break;
        }

        GameObject ball = ballSpawner.SpawnBall(player, stage3BallDamage);


        if (ball == null)
        {
            yield break;
        }

        while (ball != null)
        {
            yield return null;
        }
    }

    // 3面パターン4
    IEnumerator Stage3Pattern4()
    {
        if (missileSpawner == null)
        {
            Debug.LogError("MissileSpawnerが設定されていません");
            yield break;
        }

        GameObject missile = missileSpawner.SpawnMissile(player,stage3MissileDamage);

        if (missile == null)
        {
            yield break;
        }

        while (missile != null)
        {
            yield return null;
        }
    }

    // 3面パターン5
    IEnumerator Stage3Pattern5()
    {
        if (missileSpawner == null)
        {
            Debug.LogError("MissileSpawnerが設定されていません");
            yield break;
        }

        float interval = 0.5f;

        // 左下から上へ
        missileSpawner.SpawnDirectionalMissile(new Vector3(-1.75f, -8f, 0f),Vector2.up,stage3MissileDamage);

        yield return new WaitForSeconds(interval);

        // 左から右へ
        missileSpawner.SpawnDirectionalMissile(new Vector3(-8f, -3.5f, 0f),Vector2.right,stage3MissileDamage);

        yield return new WaitForSeconds(interval);

        // 右上から下へ
        missileSpawner.SpawnDirectionalMissile(new Vector3(1.75f, 8f, 0f),Vector2.down,stage3MissileDamage);

        yield return new WaitForSeconds(interval);

        // 右から左へ
        missileSpawner.SpawnDirectionalMissile(new Vector3(8f, -0.5f, 0f),Vector2.left,stage3MissileDamage);
    }

    [Header("Boss撃破演出")]
    [SerializeField] private float bossDefeatDelay = 0.5f;
    [SerializeField] private float sceneChangeDelay = 1.1f;

    public IEnumerator BossDefeated()
    {
        bossDefeated = true;

        if (bossPatternCoroutine != null)
        {
            StopCoroutine(bossPatternCoroutine);
            bossPatternCoroutine = null;
        }

        yield return new WaitForSeconds(bossDefeatDelay);

        if (enemyDisplayManager != null)
        {
            enemyDisplayManager.HideEnemy();
        }

        yield return new WaitForSeconds(sceneChangeDelay);

        SceneManager.LoadScene("gameclear");
    }
}