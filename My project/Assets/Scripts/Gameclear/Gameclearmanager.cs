using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
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

    // 次のステージへ
    public void NextStage()
    {
        StartCoroutine(GoToNextStage());
    }

    private IEnumerator GoToNextStage()
    {
        // クリック音
        audioSource.PlayOneShot(clickSE);

        // 少し待つ
        yield return new WaitForSeconds(sceneChangeDelay);

        // ステージ番号を1増やす
        StageManager.CurrentStage++;

        // チュートリアル完了
        Tutorial.onTutorialComplete = true;
        Tutorial.Instance.onTutorial = false;
        Life.Instance.lifedefinition(50);

        // 次のシーンへ移動
        SceneManager.LoadScene("Main");
    }
}