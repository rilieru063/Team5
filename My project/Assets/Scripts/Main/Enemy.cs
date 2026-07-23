using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public GridLines grid;
    private MovePlayer player;

    private int gridX;
    private int gridY;

    void Start()
    {
        grid = FindFirstObjectByType<GridLines>();
        player = FindFirstObjectByType<MovePlayer>();

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
                    break;
                }

                queue.Enqueue(new Vector2Int(nextX, nextY));
            }

            if (goal != Vector2Int.zero)
                break;
        }
    }
}

