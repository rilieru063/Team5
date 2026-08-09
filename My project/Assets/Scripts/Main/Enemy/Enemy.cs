using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GridLines grid;
    private MovePlayer player;

    private int gridX;
    private int gridY;

    public int GridX => gridX;
    public int GridY => gridY;

    private int startX;
    private int startY;

    private int previousX;
    private int previousY;

    public int PreviousX => previousX;
    public int PreviousY => previousY;

    private int stopTurn = 0;
    public bool IsStopped()
    {
        return stopTurn > 0;
    }

    void Start()
    {
        grid = FindFirstObjectByType<GridLines>();
        player = FindFirstObjectByType<MovePlayer>();

        EnemyManager.Instance.RegisterEnemy(this);
    }

    void Update()
    {
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

        transform.position = grid.GetCellCenter(gridX, gridY);
    }

    void CheckPlayerCollision()
    {
        if (gridX != player.GridX ||
            gridY != player.GridY)
            return;

        if (ItemManager.Instance.IsKnockbackActive())
        {
            int dx = gridX - previousX;
            int dy = gridY - previousY;

            KnockBack(-dx, -dy);
            return;
        }

        if (DebugMode.Instance.invincible)
            return;

        Debug.Log("Game Over!");
        SceneManager.LoadScene("Battle");
    }
    void CheckTrapCollision()
    {
        ItemTrap[] traps = FindObjectsByType<ItemTrap>(
            FindObjectsSortMode.None
        );

        foreach (ItemTrap trap in traps)
        {
            if (gridX == trap.GridX &&
                gridY == trap.GridY)
            {
                Debug.Log("EnemyがTrapを踏んだ！");

                StopEnemy(5);

                Destroy(trap.gameObject);

                return;
            }
        }
    }

    public void KnockBack(int directionX, int directionY)
    {
        for (int i = 0; i < 5; i++)
        {
            int nextX = gridX + directionX;
            int nextY = gridY + directionY;

            // マップ外なら終了
            if (nextX < 0 || nextY < 0 ||
                nextX >= MapLoader.Instance.mapData.GetLength(0) ||
                nextY >= MapLoader.Instance.mapData.GetLength(1))
            {
                break;
            }

            // 壁なら終了
            if (MapLoader.Instance.mapData[nextX, nextY] == (int)TileType.Wall)
            {
                break;
            }

            SetGridPosition(nextX, nextY);
        }
    }
    public void StopEnemy(int turn)
    {
        stopTurn = turn;
    }
    public void CountStopTurn()
    {
        if (stopTurn <= 0)
            return;

        stopTurn--;

        Debug.Log($"Enemy停止中 残り{stopTurn}歩");
    }

    public void MoveEnemy()
    {
        if (stopTurn > 0)
            return;

        bool[,] visited = new bool[
            MapLoader.Instance.mapData.GetLength(0),
            MapLoader.Instance.mapData.GetLength(1)
        ];
        Vector2Int[,] parent = new Vector2Int[
            MapLoader.Instance.mapData.GetLength(0),
            MapLoader.Instance.mapData.GetLength(1)
        ];
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(new Vector2Int(gridX, gridY));
        visited[gridX, gridY] = true;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        bool found = false;
        Vector2Int goal = Vector2Int.zero;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int nextX = current.x + dx[i];
                int nextY = current.y + dy[i];

                // マップ外ならスキップ
                if (nextX < 0 || nextY < 0 ||
                    nextX >= MapLoader.Instance.mapData.GetLength(0) ||
                    nextY >= MapLoader.Instance.mapData.GetLength(1))
                {
                    continue;
                }

                if (visited[nextX, nextY])
                    continue;
                // 壁ならスキップ
                if (MapLoader.Instance.mapData[nextX, nextY] == (int)TileType.Wall)
                    continue;
                visited[nextX, nextY] = true;
                parent[nextX, nextY] = current;
                if (nextX == player.GridX && nextY == player.GridY)
                {
                    goal = new Vector2Int(nextX, nextY);
                    found = true;
                    break;
                }

                queue.Enqueue(new Vector2Int(nextX, nextY));
            }

            if (found)
                break;
        }
        // プレイヤーが見つからなかったら終了
        if (!found)
            return;

        Vector2Int step = goal;

        // 敵の隣のマスまで戻る
        while (parent[step.x, step.y] != new Vector2Int(gridX, gridY))
        {
            step = parent[step.x, step.y];
        }

        previousX = gridX;
        previousY = gridY;

        // 1マス移動
        //Debug.Log($"Enemy Move : ({step.x}, {step.y})");
        SetGridPosition(step.x, step.y);
        // ItemTrapと重なったか
        CheckTrapCollision();
        // プレイヤーと重なったらゲームオーバー
        CheckPlayerCollision();
    }
}

