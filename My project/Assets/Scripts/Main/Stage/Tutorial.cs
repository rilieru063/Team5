using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image tutorialImage;
    public Sprite[] tutorialPages;

    private int currentPage = 0;

    void Start()
    {
        tutorialImage.sprite = tutorialPages[0];
    }

    public void NextPage()
    {
        currentPage++;

        if (currentPage >= tutorialPages.Length)
        {
            currentPage = tutorialPages.Length - 1;
            return;
        }

        tutorialImage.sprite = tutorialPages[currentPage];
    }

    public void PreviousPage()
    {
        currentPage--;

        if (currentPage < 0)
        {
            currentPage = 0;
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