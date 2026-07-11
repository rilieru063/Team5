using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneManager : MonoBehaviour
{
    // リトライ
    public void Retry()
    {
        SceneManager.LoadScene("Main");
    }

    // タイトルへ戻る
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}