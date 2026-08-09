using UnityEngine;
using System.IO;
using System.Collections.Generic;

public enum TileType
{
    Floor = 0,
    Wall = 1,
    Player = 2,
    Goal = 3,
    EnemySpawn = 4,
    ItemBoxSpawn = 5,
}

public class MapLoader : MonoBehaviour
{
    public static MapLoader Instance;
    public int[,] mapData;
    public GridLines grid;
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject goalPrefab;
    public GameObject enemyPrefab;
    public GameObject itemBoxPrefab;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log("LoadMap");
        LoadMap();
    }

    void LoadMap()
    {
        string fileName = $"Stage{StageManager.CurrentStage:D2}.csv";
        List<Vector2Int> enemySpawns = new List<Vector2Int>();

        string path = Path.Combine(
            Application.streamingAssetsPath,
            "StageData",
            fileName
        );

        if (!File.Exists(path))
        {
            Debug.LogError($"CSVが見つかりません : {path}");
            return;
        }

        string csvText = File.ReadAllText(path);

        string[] lines = csvText.Replace("\r", "").Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

        mapData = new int[lines[0].Trim().Split(',').Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            int mapY = lines.Length - 1 - y;

            string[] cells = lines[y].Trim().Split(',');

            for (int x = 0; x < cells.Length; x++)
            {
                int id = 0; // デフォルトは床

                if (!string.IsNullOrWhiteSpace(cells[x]))
                {
                    id = int.Parse(cells[x]);
                }

                mapData[x, mapY] = id;

                Vector2 pos = grid.GetCellCenter(x, mapY);

                switch ((TileType)id)
                {
                    case TileType.Wall:
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        break;

                    case TileType.Player:
                        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);

                        MovePlayer move = player.GetComponent<MovePlayer>();
                        move.SetStartPosition(x, mapY);
                        break;

                    case TileType.Goal:
                        GameObject goal = Instantiate(goalPrefab, pos, Quaternion.identity);

                        Goal goalScript = goal.GetComponent<Goal>();
                        goalScript.SetGridPosition(x, mapY);
                        break;

                    case TileType.EnemySpawn:
                        enemySpawns.Add(new Vector2Int(x, mapY));
                        break;

                    case TileType.ItemBoxSpawn:
                        GameObject itemBox =
                            Instantiate(itemBoxPrefab, pos, Quaternion.identity);

                        ItemBox itemBoxScript =
                            itemBox.GetComponent<ItemBox>();

                        itemBoxScript.SetGridPosition(x, mapY);
                        break;
                }
            }
        }
        // 敵のスポーン地点からランダムで1か所選ぶ
        if (enemySpawns.Count > 0)
        {
            int index = Random.Range(0, enemySpawns.Count);
            Vector2Int spawn = enemySpawns[index];

            Vector2 pos = grid.GetCellCenter(spawn.x, spawn.y);

            GameObject enemy = Instantiate(
                enemyPrefab,
                pos,
                Quaternion.identity
            );

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.SetStartPosition(spawn.x, spawn.y);
        }
    }
}