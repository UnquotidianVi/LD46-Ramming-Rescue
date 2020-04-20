using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button startGameButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button helpButton;
    [SerializeField]
    private Button creditsButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject helpMenu;
    [SerializeField]
    private GameObject creditMenu;
    [SerializeField]
    private Sound buttonPressSound;
    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private GameManager gameManager;

    public void OnEnable()
    {
        if (gameManager.GamePausedOnTheBackground)
            continueButton.gameObject.SetActive(true);
        else
            continueButton.gameObject.SetActive(false);
    }

    public void OnStartGameButtonPressed()
    {
        gameManager.StartNewGame();
        soundManager.PlaySound(buttonPressSound);
    }

    public void OnSettingsButtonPressed()
    {
        OpenSettingsMenu();
        soundManager.PlaySound(buttonPressSound);
    }

    public void OnHelpButtonPressed()
    {
        OpenHelpMenu();
        soundManager.PlaySound(buttonPressSound);
    }

    public void OnCreditsButtonPressed()
    {
        ShowCredits();
        soundManager.PlaySound(buttonPressSound);
    }

    public void OnExitButtonPressed()
    {
        soundManager.PlaySound(buttonPressSound);
        HideMenuButtons();
        Application.Quit();
    }

    public void OnContinueButtonPressed()
    {
        soundManager.PlaySound(buttonPressSound);
        gameManager.ContinueGame();
    }

    private void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        HideMenuButtons();
    }

    private void OpenHelpMenu()
    {
        helpMenu.SetActive(true);
        HideMenuButtons();
    }

    private void ShowCredits()
    {
        creditMenu.SetActive(true);
        HideMenuButtons();
    }

    public void HideMenuButtons()
    {
        startGameButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        helpButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
    }

    public void ShowMenuButtons()
    {
        startGameButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        helpButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        if (gameManager.GamePausedOnTheBackground)
            continueButton.gameObject.SetActive(true);
    }
}
