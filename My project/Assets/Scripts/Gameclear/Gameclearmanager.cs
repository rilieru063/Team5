
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    [Header("効果音")]
    public AudioSource audioSource;
    public AudioClip hoverSE;
    public AudioClip clickSE;
    public AudioClip clearSE;

    [Header("クリア画面")]
    public GameObject clearUI;       // 通常クリア
    public GameObject allClearUI;    // 最終クリア

    [Header("ステージ画像")]
    public Image stageImage;
    public Sprite[] stageSprites;

    [Header("フェード")]
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1.0f;
    public float stageImageDuration = 2.0f;

    // 最終ステージ番号
    private const int FinalStage = 3;

    private bool isChangingScene = false;

    private void Start()
    {
        // 時間を通常に戻す
        Time.timeScale = 1f;

        // ステージ画像は最初は非表示
        if (stageImage != null)
        {
            stageImage.gameObject.SetActive(false);
        }

        // フェードパネルを透明にする
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }

        // 最終ステージをクリアしたか判定
        bool isFinalClear =
            StageManager.CurrentStage >= FinalStage;

        // 通常クリア画面の切り替え
        if (clearUI != null)
        {
            clearUI.SetActive(!isFinalClear);
        }

        // 最終クリア画面の切り替え
        if (allClearUI != null)
        {
            allClearUI.SetActive(isFinalClear);
        }

        // クリアSE
        if (audioSource != null && clearSE != null)
        {
            audioSource.PlayOneShot(clearSE);
        }
    }

    // ボタンにカーソルを乗せたとき
    public void PlayHoverSE()
    {
        if (isChangingScene) return;

        if (audioSource != null && hoverSE != null)
        {
            audioSource.PlayOneShot(hoverSE);
        }
    }

    // NEXT STAGEボタン
    public void NextStage()
    {
        // 最終ステージでは次に進まない
        if (StageManager.CurrentStage >= FinalStage)
        {
            return;
        }

        if (isChangingScene) return;

        isChangingScene = true;
        StartCoroutine(NextStageSequence());
    }

    // REPLAYボタン
    public void Replay()
    {
        if (isChangingScene) return;

        isChangingScene = true;

        ResetLife();

        StartCoroutine(ChangeScene("Main"));
    }

    // TITLEボタン
    // 通常クリア・最終クリア共通
    public void BackToTitle()
    {
        if (isChangingScene) return;

        isChangingScene = true;

        ResetLife();

        StartCoroutine(BackToTitleSequence());
    }

    // 体力を初期化
    private void ResetLife()
    {
        if (Life.Instance != null)
        {
            Life.Instance.lifedefinition(50);
        }
    }

    // タイトルへ戻る
    private IEnumerator BackToTitleSequence()
    {
        PlayClickSE();

        yield return new WaitForSecondsRealtime(0.3f);

        // ステージを最初に戻す
        StageManager.CurrentStage = 0;

        // チュートリアルもリセット
        Tutorial.onTutorialComplete = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene("Title");
    }

    // 次のステージへ進む
    private IEnumerator NextStageSequence()
    {
        PlayClickSE();

        // 画面を暗くする
        yield return StartCoroutine(Fade(0f, 1f));

        // 通常クリアUIを非表示
        if (clearUI != null)
        {
            clearUI.SetActive(false);
        }

        // 次のステージ画像を表示
        if (stageImage != null &&
            stageSprites != null &&
            StageManager.CurrentStage >= 0 &&
            StageManager.CurrentStage < stageSprites.Length)
        {
            stageImage.sprite =
                stageSprites[StageManager.CurrentStage];

            stageImage.gameObject.SetActive(true);
        }

        // 画面を明るくする
        yield return StartCoroutine(Fade(1f, 0f));

        // ステージ画像を表示して待機
        yield return new WaitForSecondsRealtime(
            stageImageDuration
        );

        // 再び暗くする
        yield return StartCoroutine(Fade(0f, 1f));

        // ステージを1つ進める
        StageManager.CurrentStage++;

        // チュートリアル完了状態にする
        Tutorial.onTutorialComplete = true;

        Time.timeScale = 1f;

        // メインシーンへ
        SceneManager.LoadScene("Main");
    }

    // シーン変更
    private IEnumerator ChangeScene(string sceneName)
    {
        PlayClickSE();

        yield return new WaitForSecondsRealtime(0.3f);

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    // クリックSE再生
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

            fadeCanvasGroup.alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                time / duration
            );

            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}
