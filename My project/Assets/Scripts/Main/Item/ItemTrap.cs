using UnityEngine;

public class ItemTrap : MonoBehaviour
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
    public void Activate()
    {
        Debug.Log("Trap”­“®I");

        // “G‚ğ5•à~‚ß‚é
        // ‚±‚ÌŒã‚±‚±‚©‚çˆ—‚ğ’Ç‰Á‚·‚é
    }
}