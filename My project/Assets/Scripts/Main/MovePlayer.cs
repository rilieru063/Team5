using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MovePlayer : MonoBehaviour
{
    public GridLines grid;

    private int gridX;
    private int gridY;

    void Start()
    {
        // ç≈èâÇÃÉ}ÉX
        gridX = 1;
        gridY = 1;

        transform.position = grid.GetCellCenter(gridX, gridY);

        grid = FindFirstObjectByType<GridLines>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            gridY++;
            UpdatePosition();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            gridX--;
            UpdatePosition();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            gridY--;
            UpdatePosition();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            gridX++;
            UpdatePosition();
        }
    }

    void UpdatePosition()
    {
        transform.position = grid.GetCellCenter(gridX, gridY);
    }

    public void SetGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;

        if (grid == null)
            grid = FindFirstObjectByType<GridLines>();

        transform.position = grid.GetCellCenter(gridX, gridY);
    }
}
