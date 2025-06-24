using UnityEngine;

public class HowToPlayPagination : MonoBehaviour
{
    public GameObject[] pages;
    public GameObject buttonNext;
    public GameObject buttonPrevious;
    private int currentPage;

    private void Start()
    {
        HideAll();
        ShowPage(currentPage);
    }

    public void HideAll()
    {
        foreach (var page in pages) page.SetActive(false);
    }

    public void ShowPage(int page)
    {
        HideAll();
        pages[page].SetActive(true);
        currentPage = page;
        if (currentPage == 0)
            buttonPrevious.SetActive(false);
        else
            buttonPrevious.SetActive(true);
        if (currentPage == pages.Length - 1)
            buttonNext.SetActive(false);
        else
            buttonNext.SetActive(true);
    }

    public void NextPage()
    {
        ShowPage(currentPage + 1);
    }

    public void PreviousPage()
    {
        ShowPage(currentPage - 1);
    }
}