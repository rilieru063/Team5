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

    // タイトルBGM
    public AudioSource bgmAudioSource;

    // チュートリアル画像
    public GameObject tutorialImage;

    // チュートリアル画像を表示する時間
    public float tutorialImageDuration = 2.0f;

    private bool isStarting = false;

    public void PlayHoverSE()
    {
        if (!isStarting)
        {
            audioSource.PlayOneShot(hoverSE);
        }
    }

    public void GoToHowToPlay()
    {
        if (!isStarting)
        {
            StartCoroutine(StartSequence());
        }
    }

    private IEnumerator StartSequence()
    {
        isStarting = true;

        // STARTのSE
        audioSource.PlayOneShot(startSE);

        // BGMをフェードアウト開始
        StartCoroutine(FadeOutBGM());

        // ① タイトル画面 → 黒
        yield return StartCoroutine(Fade(0f, 1f));

        // ② 真っ黒な間にチュートリアル画像を表示
        tutorialImage.SetActive(true);

        // ③ 黒 → チュートリアル画像
        yield return StartCoroutine(Fade(1f, 0f));

        // ④ チュートリアル画像を表示
        yield return new WaitForSeconds(tutorialImageDuration);

        // ⑤ チュートリアル画像 → 黒
        yield return StartCoroutine(Fade(0f, 1f));

        // ⑥ ゲーム開始
        SceneManager.LoadScene("Main");
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;

        fadeCanvasGroup.alpha = startAlpha;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            fadeCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }

    private IEnumerator FadeOutBGM()
    {
        float startVolume = bgmAudioSource.volume;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            bgmAudioSource.volume =
                Mathf.Lerp(startVolume, 0f, time / fadeDuration);

            yield return null;
        }

        bgmAudioSource.volume = 0f;
        bgmAudioSource.Stop();
    }
}