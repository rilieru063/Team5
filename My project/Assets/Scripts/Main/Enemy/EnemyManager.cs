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
            if (enemy.IsStopped())
            {
                enemy.CountStopTurn();
                continue;
            }

            enemy.MoveEnemy();
            Life.Instance.lifeminus(1);
            Life.Instance.RefreshUI();

            enemy.MoveEnemy();
            Life.Instance.lifeminus(1);
            Life.Instance.RefreshUI();
        }

        ItemManager.Instance.EnemyTurn();
    }
}