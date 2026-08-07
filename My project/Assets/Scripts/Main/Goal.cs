using UnityEngine;

public class Goal : MonoBehaviour
{
    private int gridX;
    private int gridY;

    public int GridX => gridX;
    public int GridY => gridY;

    public void SetGridPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
    }
}