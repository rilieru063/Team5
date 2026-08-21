using UnityEngine;

public class Life : MonoBehaviour
{
    public static Life Instance;

    public int lifepoint;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void lifeminus(int minus) //ライフをマイナス
    {
        lifepoint -= minus;
        Debug.Log(lifepoint);
    }
    
    public void lifedefinition(int num) //ライフを強制
    {
        lifepoint = num;
        Debug.Log(lifepoint);
    }
}
