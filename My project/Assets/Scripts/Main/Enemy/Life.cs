using TMPro;
using UnityEngine;

public class Life : MonoBehaviour
{
    public static Life Instance;

    public int lifepoint;
    public TextMeshProUGUI LifeText;

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

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if(LifeText != null)
        {
            LifeText.text = lifepoint.ToString();
        }
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
