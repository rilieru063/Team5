using System.Diagnostics;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public static DebugMode Instance;

    [Header("デバッグ設定")]
    public bool Alldebugmode = false;  //全てのデバッグ機能
    public bool debug = false;         //デバッグ機能
    public bool showEnemyPath = false; //敵の経路表示
    public bool invincible = false;    //無敵

    void Awake()
    {
        Instance = this;
        Enemy enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        if(Alldebugmode == true)
        {
            debug = Alldebugmode;
            showEnemyPath = Alldebugmode;
            invincible = Alldebugmode;
        }

        if (!debug)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            MovePlayer player = FindFirstObjectByType<MovePlayer>();
            player.ResetPosition();

            foreach (Enemy enemy in EnemyManager.Instance.Enemies)
            {
                enemy.ResetPosition();
            }
        }
    }
}