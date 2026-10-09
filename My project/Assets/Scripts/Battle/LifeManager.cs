using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;
    public Image hpBar;
    public int maxLife = 100;
    public int life;

    public AudioSource audioSource;
    public AudioClip damage;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        life = maxLife;
        UpdateLifeUI();
    }

    public void Damage(int damageAmount)
    {
        int previousLife = life;

        life -= damageAmount;

        if (life < 0)
            life = 0;

        // ŽÀÛ‚Élife‚ªŒ¸‚Á‚½‚Æ‚«‚¾‚¯‰¹‚ðÄ¶
        if (life < previousLife)
        {
            audioSource.PlayOneShot(damage);
        }

        UpdateLifeUI();

        if (life <= 0)
        {
            GameOver();
        }
    }

    void UpdateLifeUI()
    {
        hpBar.fillAmount = (float)life / maxLife;
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
