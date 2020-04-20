using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> pages = new List<GameObject>();
    [SerializeField]
    private Text currentPageText;
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private Sound pageOpenSound;
    [SerializeField]
    private Sound buttonPressSound;

    private int currentPage;

    private void OnEnable()
    {
        if (pages.Count == 0)
            return;

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }

        pages[0].SetActive(true);
        currentPage = 0;
        currentPageText.text = (currentPage + 1) + "/" + pages.Count;
    }

    public void OnNextPageButtonPressed()
    {
        soundManager.PlaySound(pageOpenSound);
        ChangePageTo(currentPage + 1);
    }

    public void OnPreviousPageButtonPressed()
    {
        soundManager.PlaySound(pageOpenSound);
        ChangePageTo(currentPage - 1);
    }

    public void OnExitButtonPressed()
    {
        mainMenu.ShowMenuButtons();
        soundManager.PlaySound(buttonPressSound);
        gameObject.SetActive(false);
    }

    private void ChangePageTo(int page)
    {
        if (page == -1)
            page = pages.Count - 1;
        if (page == pages.Count)
            page = 0;

        pages[currentPage].SetActive(false);
        pages[page].SetActive(true);
        currentPage = page;

        currentPageText.text = (page + 1) + "/" + pages.Count;
    }
}
