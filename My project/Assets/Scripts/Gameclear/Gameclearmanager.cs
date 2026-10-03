using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSE;
    public AudioClip clickSE;

    public float sceneChangeDelay = 0.3f;

    // カーソルがボタンに乗ったとき
    public void PlayHoverSE()
    {
        audioSource.PlayOneShot(hoverSE);
    }

    // 次のステージへ
    public void NextStage()
    {
        StartCoroutine(GoToNextStage());
    }

    // 今のステージをもう一度
    public void Replay()
    {
        Life.Instance.lifedefinition(50);
        StartCoroutine(ChangeScene("Main"));
    }

    // タイトルへ戻る
    public void BackToTitle()
    {
        Life.Instance.lifedefinition(50);
        StartCoroutine(ChangeScene("Title"));
    }

    // 次のステージへ進む処理
    private IEnumerator GoToNextStage()
    {
        audioSource.PlayOneShot(clickSE);

        yield return new WaitForSeconds(sceneChangeDelay);

        // 次のステージ番号へ
        StageManager.CurrentStage++;

        // チュートリアル完了
        Tutorial.onTutorialComplete = true;
        Tutorial.Instance.onTutorial = false;
        Life.Instance.lifedefinition(50);

        SceneManager.LoadScene("Main");
    }

    // リプレイ・タイトル用
    private IEnumerator ChangeScene(string sceneName)
    {
        audioSource.PlayOneShot(clickSE);

        yield return new WaitForSeconds(sceneChangeDelay);

        SceneManager.LoadScene(sceneName);
    }
}