using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<Enemy> enemies = new List<Enemy>();

    public List<Enemy> Enemies
    {
        get { return enemies; }
    }
    void Awake()
    {
        Instance = this;
        enemies.Clear();
    }

    public void RegisterEnemy(Enemy enemy)
    {
        Debug.Log($"RegisterEnemy : {enemy}");
        enemies.Add(enemy);
    }

    public void MoveEnemies()
    {
        if (Life.Instance.lifepoint == 0)
            return;

        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
            {
                Debug.LogError("Enemy‚ªnull‚Å‚·");
                continue;
            }

            enemy.MoveEnemy();
            Life.Instance.lifeminus(1);
            enemy.MoveEnemy();
            Life.Instance.lifeminus(1);
            Debug.Log(ItemManager.Instance.KBA);
            Debug.Log(ItemManager.Instance.KBT);
        }

        ItemManager.Instance.EnemyTurn();
    }
}