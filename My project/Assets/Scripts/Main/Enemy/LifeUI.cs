using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    void Start()
    {
        if (Life.Instance == null)
        {
            Debug.LogError("Life.Instance‚ª‚ ‚è‚Ü‚¹‚ñ");
            return;
        }

        Life.Instance.LifeText = lifeText;
        Life.Instance.RefreshUI();
    }

    //void OnEnable()
    //{
    //    if (Life.Instance == null)
    //    {
    //        Debug.LogError("Life.Instance‚ª‚ ‚è‚Ü‚¹‚ñ");
    //        return;
    //    }

    //    Debug.Log("LifeUI OnEnable : " + Life.Instance.lifepoint);

    //    Life.Instance.LifeText = lifeText;
    //    Life.Instance.RefreshUI();

    //    Debug.Log("•\Ž¦Œã : " + lifeText.text);
    //}
}