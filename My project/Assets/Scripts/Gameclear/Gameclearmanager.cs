using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSE;
    public AudioClip clickSE;
    public AudioClip clearSE; // ★ ゲームクリア時のSE

    // UI
    public GameObject clearUI;
    public Image stageImage;
    public Sprite[] stageSprites;
    public CanvasGroup fadeCanvasGroup;

    // 時間調整
    public float fadeDuration = 1.0f;
    public float stageImageDuration = 2.0f;

    private void Start()
    {
        // 最初はステージ画像を隠す
        stageImage.gameObject.SetActive(false);

        // FadePanelは透明
        fadeCanvasGroup.alpha = 0f;

        // ★ ゲームクリア画面に来た瞬間にSE
        audioSource.PlayOneShot(clearSE);
    }

    // カーソルを乗せたとき
    public void PlayHoverSE()
    {
        audioSource.PlayOneShot(hoverSE);
    }

    // NEXT STAGE
    public void NextStage()
    {
        StartCoroutine(NextStageSequence());
    }

    // REPLAY
    public void Replay()
    {
        Life.Instance.lifedefinition(50);
        StartCoroutine(ChangeScene("Main"));
    }

    // TITLE
    public void BackToTitle()
    {
        Life.Instance.lifedefinition(50);
        StartCoroutine(ChangeScene("Title"));
    }

    private IEnumerator NextStageSequence()
    {
        // クリックSE
        audioSource.PlayOneShot(clickSE);

        // ① 黒にフェードアウト
        yield return StartCoroutine(Fade(0f, 1f));

        // ② 真っ黒な間にGAME CLEAR画面を消す
        clearUI.SetActive(false);

        // 次のステージ画像を設定
        if (StageManager.CurrentStage >= 0 &&
            StageManager.CurrentStage < stageSprites.Length)
        {
            stageImage.sprite = stageSprites[StageManager.CurrentStage];
            stageImage.gameObject.SetActive(true);
        }

        // ③ ステージ画像へフェードイン
        yield return StartCoroutine(Fade(1f, 0f));

        // ④ ステージ画像を見せる
        yield return new WaitForSeconds(stageImageDuration);

        // ⑤ もう一度黒にフェードアウト
        yield return StartCoroutine(Fade(0f, 1f));

        // 次のステージへ
        StageManager.CurrentStage++;

        // チュートリアル完了
        Tutorial.onTutorialComplete = true;

        SceneManager.LoadScene("Main");
    }

    // REPLAY・TITLE用
    private IEnumerator ChangeScene(string sceneName)
    {
        audioSource.PlayOneShot(clickSE);

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(sceneName);
    }

    // フェード処理
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            fadeCanvasGroup.alpha =
                Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}