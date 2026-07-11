using UnityEngine;

public class GridLines : MonoBehaviour

{

    public int width = 8;

    public int height = 8;

    public float cellSize = 1f;

    void Start()

    {

        DrawGrid();

    }

    void DrawGrid()

    {

        float offsetX = (width * cellSize) / 2f;

        float offsetY = (height * cellSize) / 2f;

        // 縦線

        for (int x = 0; x <= width; x++)

        {

            CreateLine(

                new Vector2(x * cellSize - offsetX, -offsetY),

                new Vector2(x * cellSize - offsetX, height * cellSize - offsetY)

            );

        }

        // 横線

        for (int y = 0; y <= height; y++)

        {

            CreateLine(

                new Vector2(-offsetX, y * cellSize - offsetY),

                new Vector2(width * cellSize - offsetX, y * cellSize - offsetY)

            );

        }

    }

    void CreateLine(Vector2 start, Vector2 end)

    {

        GameObject line = new GameObject("GridLine");

        line.transform.parent = transform;

        LineRenderer lr = line.AddComponent<LineRenderer>();

        lr.useWorldSpace = false;

        lr.positionCount = 2;

        lr.SetPosition(0, start);

        lr.SetPosition(1, end);

        lr.startWidth = 0.03f;

        lr.endWidth = 0.03f;

        lr.material = new Material(Shader.Find("Sprites/Default"));

        lr.sortingOrder = 10;

    }

    // マス座標 → ワールド座標

    public Vector2 GetCellCenter(int x, int y)

    {

        float offsetX = (width * cellSize) / 2f;

        float offsetY = (height * cellSize) / 2f;

        return new Vector2(

            x * cellSize + cellSize / 2f - offsetX,

            y * cellSize + cellSize / 2f - offsetY

        );

    }

}

