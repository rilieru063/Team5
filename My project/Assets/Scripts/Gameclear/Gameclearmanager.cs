using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    public void NextStage()
    {
        // ステージ番号を1増やす
        StageManager.CurrentStage++;

        // 次のシーンへ移動
        SceneManager.LoadScene("Main");
    }
}