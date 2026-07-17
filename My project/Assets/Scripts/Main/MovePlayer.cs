using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MovePlayer : MonoBehaviour
{
    public GridLines grid;

    private int gridX;
    private int gridY;

    void Start()
    {
        grid = FindFirstObjectByType<GridLines>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(0, 1);
        if (Input.GetKeyDown(KeyCode.S)) Move(0, -1);
        if (Input.GetKeyDown(KeyCode.A)) Move(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) Move(1, 0);
    }

    void Move(int dx, int dy)
    {
        int nextX = gridX + dx;
        int nextY = gridY + dy;

        if (nextX < 0 || nextY < 0 ||
            nextX >= MapLoader.Instance.mapData.GetLength(0) ||
            nextY >= MapLoader.Instance.mapData.GetLength(1))
            return;

        if (MapLoader.Instance.mapData[nextX, nextY] == (int)TileType.Wall)
            return;

        SetGridPosition(nextX, nextY);
    }

    void UpdatePosition()
    {
        //transform.position = grid.GetCellCenter(gridX, gridY);
        Vector2 pos = grid.GetCellCenter(gridX, gridY);

        Debug.DrawLine(pos + Vector2.left * 0.1f, pos + Vector2.right * 0.1f, Color.red, 100);
        Debug.DrawLine(pos + Vector2.up * 0.1f, pos + Vector2.down * 0.1f, Color.red, 100);

        transform.position = pos;
    }

    public void SetGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;

        if (grid == null)
            grid = FindFirstObjectByType<GridLines>();

        UpdatePosition();
    }
}
