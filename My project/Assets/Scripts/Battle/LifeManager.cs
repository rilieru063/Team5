using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    public GameObject[] LifeObjects;
    public int life = 3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateLifeUI();
    }

    public void Damage(int Damage)
    {
        life -= Damage;

        if(life < 0 )
            life = 0;

        UpdateLifeUI();

        Debug.Log("Œ»Ý‚Ìƒ‰ƒCƒtF" + life);
    }

    void UpdateLifeUI()
    {
        for (int i = 0; i < LifeObjects.Length; i++)
        {
            LifeObjects[i].SetActive(i < life);
        }
    }
}
