
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    // 効果音
    public AudioSource audioSource;
    public AudioClip hoverSE;
    public AudioClip clickSE;
    public AudioClip clearSE;

    // UI
    public GameObject clearUI;
    public Image stageImage;
    public Sprite[] stageSprites;
    public CanvasGroup fadeCanvasGroup;

    // 時間調整
    public float fadeDuration = 1.0f;
    public float stageImageDuration = 2.0f;

    private bool isChangingScene = false;

    private void Start()
    {
        // 最初はステージ画像を隠す
        if (stageImage != null)
        {
            stageImage.gameObject.SetActive(false);
        }

        // FadePanelは透明
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }

        // ゲームクリアSE
        if (audioSource != null && clearSE != null)
        {
            audioSource.PlayOneShot(clearSE);
        }
    }

    // カーソルを乗せたとき
    public void PlayHoverSE()
    {
        if (!isChangingScene &&
            audioSource != null &&
            hoverSE != null)
        {
            audioSource.PlayOneShot(hoverSE);
        }
    }

    // NEXT STAGE
    public void NextStage()
    {
        if (isChangingScene) return;

        isChangingScene = true;
        StartCoroutine(NextStageSequence());
    }

    // REPLAY
    public void Replay()
    {
        if (isChangingScene) return;

        isChangingScene = true;

        // Lifeが存在する場合だけ体力回復
        ResetLife();

        StartCoroutine(ChangeScene("Main"));
    }

    // TITLE
    public void BackToTitle()
    {
        if (isChangingScene) return;

        isChangingScene = true;

        // Lifeが存在する場合だけ体力回復
        ResetLife();

        StartCoroutine(BackToTitleSequence());
    }

    // 体力を50に戻す
    private void ResetLife()
    {
        if (Life.Instance != null)
        {
            Life.Instance.lifedefinition(50);
        }
    }

    // TITLEに戻る専用処理
    private IEnumerator BackToTitleSequence()
    {
        // クリックSE
        PlayClickSE();

        yield return new WaitForSecondsRealtime(0.3f);

        // ★ ステージ数を最初に戻す
        StageManager.CurrentStage = 0;

        // ★ チュートリアルも初期状態へ
        Tutorial.onTutorialComplete = false;

        // タイトル画面へ
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    // NEXT STAGEの演出
    private IEnumerator NextStageSequence()
    {
        // クリックSE
        PlayClickSE();

        // ① 黒にフェードアウト
        yield return StartCoroutine(Fade(0f, 1f));

        // ② GAME CLEAR画面を消す
        if (clearUI != null)
        {
            clearUI.SetActive(false);
        }

        // 次のステージ画像を設定
        if (stageImage != null &&
            stageSprites != null &&
            StageManager.CurrentStage >= 0 &&
            StageManager.CurrentStage < stageSprites.Length)
        {
            stageImage.sprite = stageSprites[StageManager.CurrentStage];
            stageImage.gameObject.SetActive(true);
        }

        // ③ ステージ画像へフェードイン
        yield return StartCoroutine(Fade(1f, 0f));

        // ④ ステージ画像を表示
        yield return new WaitForSecondsRealtime(stageImageDuration);

        // ⑤ 黒にフェードアウト
        yield return StartCoroutine(Fade(0f, 1f));

        // 次のステージへ
        StageManager.CurrentStage++;

        // チュートリアル完了
        Tutorial.onTutorialComplete = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    // REPLAY用
    private IEnumerator ChangeScene(string sceneName)
    {
        PlayClickSE();

        yield return new WaitForSecondsRealtime(0.3f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    // クリックSE
    private void PlayClickSE()
    {
        if (audioSource != null && clickSE != null)
        {
            audioSource.PlayOneShot(clickSE);
        }
    }

    // フェード処理
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        float time = 0f;
        float duration = Mathf.Max(0.01f, fadeDuration);

        fadeCanvasGroup.alpha = startAlpha;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            fadeCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, endAlpha, time / duration);

            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}
