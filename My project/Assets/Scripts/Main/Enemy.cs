using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public GridLines grid;
    private MovePlayer player;

    private int gridX;
    private int gridY;

    private int startX;
    private int startY;

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
    public void MoveEnemy()
    {
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
                    parent[nextX, nextY] = current;
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

        // 1マス移動
        Debug.Log($"Enemy Move : ({step.x}, {step.y})");
        SetGridPosition(step.x, step.y);
    }
}

