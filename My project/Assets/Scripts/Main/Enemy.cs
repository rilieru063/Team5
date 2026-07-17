using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Enemy : MonoBehaviour
{
    public GridLines grid;

    private int gridX;
    private int gridY;

    void Start()
    {
        grid = FindFirstObjectByType<GridLines>();

        EnemyManager.Instance.RegisterEnemy(this);
    }

    void Update()
    {
        
    }

    public void SetGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;

        if (grid == null)
            grid = FindFirstObjectByType<GridLines>();
    }

    public void MoveEnemy()
    {
        SetGridPosition(gridX + 1, gridY);
    }
}
