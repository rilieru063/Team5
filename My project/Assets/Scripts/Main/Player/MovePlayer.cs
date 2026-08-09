using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public GridLines grid;

    private int gridX;
    private int gridY;

    private int startX;
    private int startY;

    public int GridX => gridX;
    public int GridY => gridY;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ItemManager.Instance.UseItem())
            {
                EnemyManager.Instance.MoveEnemies();
            }
        }
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

        if (ItemManager.Instance.IsDoubleMoveActive())
        {
            if (ItemManager.Instance.DoubleMoveStep())
            {
                EnemyManager.Instance.MoveEnemies();
            }
        }
        else
        {
            EnemyManager.Instance.MoveEnemies();
        }
    }

    void UpdatePosition()
    {
        //transform.position = grid.GetCellCenter(gridX, gridY);
        Vector2 pos = grid.GetCellCenter(gridX, gridY);

        //Debug.DrawLine(pos + Vector2.left * 0.1f, pos + Vector2.right * 0.1f, Color.red, 100);
        //Debug.DrawLine(pos + Vector2.up * 0.1f, pos + Vector2.down * 0.1f, Color.red, 100);

        transform.position = pos;
    }

    public void SetStartPosition(int x, int y)
    {
        startX = x;
        startY = y;

        SetGridPosition(x, y);
    }

    public void ResetPosition()
    {
        SetGridPosition(startX, startY);
    }

    public void SetGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;

        if (grid == null)
            grid = FindFirstObjectByType<GridLines>();

        UpdatePosition();
        CheckEnemyCollision();
        CheckItemBox();
        CheckGoal();
    }

    void CheckEnemyCollision()
    {
        if (DebugMode.Instance.invincible)
            return;

        foreach (Enemy enemy in EnemyManager.Instance.Enemies)
        {
            if (GridX == enemy.GridX &&
                GridY == enemy.GridY)
            {
                Debug.Log("Game Over!");
                SceneManager.LoadScene("Battle");
            }
        }
    }

    void CheckItemBox()
    {
        ItemBox[] itemBoxes = FindObjectsByType<ItemBox>(FindObjectsSortMode.None);

        foreach (ItemBox itemBox in itemBoxes)
        {
            if (GridX == itemBox.GridX &&
                GridY == itemBox.GridY)
            {
                itemBox.GetItem();
                return;
            }
        }
    }

    void CheckGoal()
    {
        Goal goal = FindFirstObjectByType<Goal>();

        if (goal == null)
            return;

        if (GridX == goal.GridX &&
            GridY == goal.GridY)
        {
            Debug.Log("Stage Clear!");
            Life.Instance.lifedefinition(1);
            SceneManager.LoadScene("Battle");
        }
    }
}
