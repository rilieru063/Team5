using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance;

    public Image tutorialImage;
    public Sprite[] tutorialPages;
    private int currentPage = 0;

    public bool onTutorial;
    public static bool onTutorialComplete = false;

    void Awake()
    {
        Instance = this;
        if (onTutorialComplete == true)
            tutorialImage.enabled = false;
    }

    void Start()
    {
        if (DebugMode.Instance != null &&
            DebugMode.Instance.tutorialComp)
        {
            onTutorialComplete = true;
            onTutorial = false;
            tutorialImage.enabled = false;
            return;
        }
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Main")
        {
            currentPage = 0;
            Pagedefinition(currentPage);
        }
        else if (sceneName == "Battle")
        {
            currentPage = 6;
            Pagedefinition(currentPage);
        }

    }

    void Update()
    {
        if (!onTutorial)
            return;
        string sceneName = SceneManager.GetActiveScene().name;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (sceneName == "Main")
                NextPage(5);
            if (sceneName == "Battle")
                NextPage(10);
        }
    }

    public void NextPage(int lastPage)
    {
        currentPage++;

        if (currentPage > lastPage)
        {
            onTutorial = false;
            tutorialImage.enabled = false;
            return;
        }

        tutorialImage.sprite = tutorialPages[currentPage];
    }

    public void Pagedefinition(int page)
    {
        if (page < 0 || page >= tutorialPages.Length)
            return;
        tutorialImage.sprite = tutorialPages[page];
    }
}