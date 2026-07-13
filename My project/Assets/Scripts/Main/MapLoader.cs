using UnityEngine;
using System.IO;

public class MapLoader : MonoBehaviour
{
    public GridLines grid;

    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject goalPrefab;

    void Start()
    {
        Debug.Log("LoadMap");
        LoadMap();

    }

    void LoadMap()
    {
        string fileName = $"Stage{StageManager.CurrentStage:D2}.csv";

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

        string[] lines = csvText.Split('\n');

        for (int y = 0; y < lines.Length; y++)
        {
            string[] cells = lines[y].Trim().Split(',');

            for (int x = 0; x < cells.Length; x++)
            {
                int id = 0; // デフォルトは床

                if (!string.IsNullOrWhiteSpace(cells[x]))
                {
                    id = int.Parse(cells[x]);
                }

                Vector2 pos = grid.GetCellCenter(x, y);

                switch (id)
                {
                    case 1:
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        break;

                    case 2:
                        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);

                        MovePlayer move = player.GetComponent<MovePlayer>();
                        move.SetGridPosition(x, y);
                        break;

                    case 3:
                        Instantiate(goalPrefab, pos, Quaternion.identity);
                        break;
                }
            }
        }
    }
}