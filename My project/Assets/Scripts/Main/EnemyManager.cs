using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<Enemy> enemies = new List<Enemy>();

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
        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
            {
                Debug.LogError("Enemy‚ªnull‚Å‚·");
                continue;
            }

            enemy.MoveEnemy();
            enemy.MoveEnemy();
        }
    }
}