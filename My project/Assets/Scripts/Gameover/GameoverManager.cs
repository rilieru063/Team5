using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    // リトライ
    public void Retry()
    {
        SceneManager.LoadScene("Main");
        Life.Instance.lifedefinition(50);
    }

    // タイトルへ戻る
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
        Life.Instance.lifedefinition(50);
    }
}