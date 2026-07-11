using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public void GoToHowToPlay()
    {
        SceneManager.LoadScene("How to play");
    }
}