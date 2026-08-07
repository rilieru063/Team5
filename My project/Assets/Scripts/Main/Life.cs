using UnityEngine;

public class Life : MonoBehaviour
{
    public static Life Instance;

    public int lifepoint;

    void Awake()
    {
        Instance = this;
    }

    public void lifeminus(int minus)
    {
        lifepoint -= minus;
        Debug.Log(lifepoint);
    }
}
