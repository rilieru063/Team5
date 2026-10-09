
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    // 効果音
    public AudioSource audioSource;
    public AudioClip hoverSE;
    public AudioClip clickSE;
    public AudioClip gameOverSE;

    // シーン切り替えまでの時間
    public float sceneChangeDelay = 0.3f;

    // ボタン連打防止
    private bool isChangingScene = false;

    // GameOver画面になった瞬間
    private void Start()
    {
        // ゲームオーバーSEを再生
        if (audioSource != null && gameOverSE != null)
        {
            audioSource.PlayOneShot(gameOverSE);
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

    // RETRY
    public void Retry()
    {
        if (isChangingScene) return;

        isChangingScene = true;

        // 体力を50に戻す
        ResetLife();

        // 同じステージから再開
        StartCoroutine(ChangeScene("Main"));
    }

    // TITLE
    public void BackToTitle()
    {
        if (isChangingScene) return;

        isChangingScene = true;

        // 体力を50に戻す
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

    // TITLE専用処理
    private IEnumerator BackToTitleSequence()
    {
        // クリックSE
        PlayClickSE();

        yield return new WaitForSecondsRealtime(sceneChangeDelay);

        // ステージ数を0に戻す
        StageManager.CurrentStage = 0;

        // チュートリアルを初期状態に戻す
        Tutorial.onTutorialComplete = false;

        // タイトル画面へ
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    // RETRY用のシーン切り替え
    private IEnumerator ChangeScene(string sceneName)
    {
        // クリックSE
        PlayClickSE();

        yield return new WaitForSecondsRealtime(sceneChangeDelay);

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
}
