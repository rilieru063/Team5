using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSE;
    public AudioClip clickSE;

    // シーン切り替えまでの待ち時間
    public float sceneChangeDelay = 0.3f;

    // カーソルがボタンに乗ったとき
    public void PlayHoverSE()
    {
        audioSource.PlayOneShot(hoverSE);
    }

    // リトライ
    public void Retry()
    {
        SceneManager.LoadScene("Main");
        Life.Instance.lifedefinition(50);
    }

    // タイトルへ戻る
    public void BackToTitle()
    {
        Life.Instance.lifedefinition(50);
        StartCoroutine(ChangeScene("Title"));
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        // クリック音
        audioSource.PlayOneShot(clickSE);

        // 少し待つ
        yield return new WaitForSeconds(sceneChangeDelay);

        // シーン移動
        SceneManager.LoadScene(sceneName);
    }
}