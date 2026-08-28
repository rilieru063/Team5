using UnityEngine;
using UnityEngine.UI;

public class BossLifeManager : MonoBehaviour
{
    public Image hpBar;

    [Header("BossÇÃç≈ëÂLifePoint")]
    public int maxLifePoint = 50;

    void Start()
    {
        UpdateLifeUI();
    }

    void Update()
    {
        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        if (Life.Instance == null)
            return;

        hpBar.fillAmount =(float)Life.Instance.lifepoint / maxLifePoint;
    }
}
