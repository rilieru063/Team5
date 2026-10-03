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

    // GameOverシーンに切り替わった瞬間
    private void Start()
    {
        // ゲームオーバーSEを鳴らす
        audioSource.PlayOneShot(gameOverSE);
    }

    // カーソルをボタンに乗せたとき
    public void PlayHoverSE()
    {
        audioSource.PlayOneShot(hoverSE);
    }

    // リトライ
    public void Retry()
    {
        StartCoroutine(ChangeScene("Main"));
    }

    // タイトルに戻る
    public void BackToTitle()
    {
        StartCoroutine(ChangeScene("Title"));
    }

    // シーン切り替え
    private IEnumerator ChangeScene(string sceneName)
    {
        // クリックSE
        audioSource.PlayOneShot(clickSE);

        // SEが鳴るのを少し待つ
        yield return new WaitForSeconds(sceneChangeDelay);

        // シーン移動
        SceneManager.LoadScene(sceneName);
    }
}