using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void MoveEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.MoveEnemy();
            enemy.MoveEnemy();   // 2ƒ}ƒXˆÚ“®
        }
    }
}