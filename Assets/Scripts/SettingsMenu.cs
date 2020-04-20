using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private Sound buttonPressedSound;
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private MainMenu mainMenu;

    private void OnEnable()
    {
        volumeSlider.value = soundManager.VolumeMultiplier;
    }

    private void Update()
    {
        UpdateChangedValues();
    }

    private void UpdateChangedValues()
    {
        if (volumeSlider.value != soundManager.VolumeMultiplier)
            soundManager.VolumeMultiplier = volumeSlider.value;
    }

    public void OnBackButtonPressed()
    {
        soundManager.PlaySound(buttonPressedSound);
        mainMenu.ShowMenuButtons();
        gameObject.SetActive(false);
    }
}
