using TMPro;
using UnityEngine;

public class Life : MonoBehaviour
{
    public static Life Instance;

    public int lifepoint;
    public TextMeshProUGUI LifeText;

    void Awake()
    {
        Debug.Log("Life Awake : " + gameObject.name);

        if (Instance != null && Instance != this)
        {
            Debug.Log("Lifeの重複を削除 : " + gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Debug.Log("Life Instance登録 : " + Instance);

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
        RefreshUI();
    }
    
    public void lifedefinition(int num) //ライフを強制
    {
        lifepoint = num;
        Debug.Log(lifepoint);
    }
}
