using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;

    public AudioSource audioSource;
    public AudioClip startSE;
    public AudioClip hoverSE;

    public void PlayHoverSE()
    {
        audioSource.PlayOneShot(hoverSE);
    }

    public void GoToHowToPlay()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        audioSource.PlayOneShot(startSE);

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        SceneManager.LoadScene("Main");
    }
}