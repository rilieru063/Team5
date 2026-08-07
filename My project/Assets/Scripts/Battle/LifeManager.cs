using UnityEngine;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;

    public Image hpBar;

    public int maxLife = 100;
    public int life;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        life = maxLife;
        UpdateLifeUI();
    }

    public void Damage(int damage)
    {
        life -= damage;

        if (life < 0)
            life = 0;

        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        hpBar.fillAmount = (float)life / maxLife;
    }
}

