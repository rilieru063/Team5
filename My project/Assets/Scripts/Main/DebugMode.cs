using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public static DebugMode Instance;

    [Header("デバッグ設定")]
    public bool Alldebugmode = false;  //全てのデバッグ機能
    public bool reset = false;         //デバッグ機能
    public bool showEnemyPath = false; //敵の経路表示
    public bool invincible = false;    //無敵
    public bool clear = false;         //強制クリア
    public bool win = false;           //バトルクリア
    public bool tutorialComp = false;  //チュートリアルスキップ

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Alldebugmode == true)
        {
            reset = Alldebugmode;
            showEnemyPath = Alldebugmode;
            invincible = Alldebugmode;
            tutorialComp = Alldebugmode;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ItemManager.Instance.ResetItem();
            MovePlayer player = FindFirstObjectByType<MovePlayer>();
            player.ResetPosition();

            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            {
                enemy.ResetPosition();
            }
        }

        if (Input.GetKey(KeyCode.LeftControl)&& Input.GetKeyDown(KeyCode.C))
        {
            clear = true;
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            win = true;
        }
            if (!reset)
            return;
    }
}