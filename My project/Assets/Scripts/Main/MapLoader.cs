using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GridLines grid;

    public TextAsset csv;

    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject goalPrefab;

    void Start()
    {
        LoadMap();
    }

    void LoadMap()
    {
        string[] lines = csv.text.Split('\n');

        for (int y = 0; y < lines.Length; y++)
        {
            string[] cells = lines[y].Split(',');

            for (int x = 0; x < cells.Length; x++)
            {
                int id = int.Parse(cells[x]);

                Vector2 pos = grid.GetCellCenter(x, y);

                switch (id)
                {
                    case 1:
                        Instantiate(wallPrefab, pos, Quaternion.identity);
                        break;

                    case 2:
                        Instantiate(playerPrefab, pos, Quaternion.identity);
                        break;

                    case 3:
                        Instantiate(goalPrefab, pos, Quaternion.identity);
                        break;
                }
            }
        }
    }
}