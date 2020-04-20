using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField]
    private MainMenu mainMenu;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private Sound buttonPressedSound;

    public void OnBackButtonPressed()
    {
        soundManager.PlaySound(buttonPressedSound);
        mainMenu.ShowMenuButtons();
        gameObject.SetActive(false);
    }
}
