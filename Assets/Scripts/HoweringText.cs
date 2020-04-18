using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoweringText : MonoBehaviour
{
    [SerializeField]
    private string content;
    [SerializeField]
    private Text howeringTextObject;
    [SerializeField]
    private GameManager.GameState visibleGameState;
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private Vector2 offsetFromObject;
    [SerializeField]
    private float textFadeoutTime;

    private float lastTimeTextWasUpdated;

    private void Start()
    {
        howeringTextObject.text = content;
    }

    private void Update()
    {
        UpdateTextPositionOnScreen();
        UpdateTextVisibility();

        if (lastTimeTextWasUpdated + textFadeoutTime < Time.time)
            howeringTextObject.text = "";
    }

    public void ShowText(string text)
    {
        howeringTextObject.text = text;
        lastTimeTextWasUpdated = Time.time;
    }

    private void UpdateTextPositionOnScreen()
    {
        Vector2 textPositionOnScreen = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + offsetFromObject.x, transform.position.y + offsetFromObject.y));
        howeringTextObject.gameObject.transform.position = textPositionOnScreen;
    }

    private void UpdateTextVisibility()
    {
        if (gameManager.CurrentGameState == visibleGameState)
            howeringTextObject.enabled = true;
        else
            howeringTextObject.enabled = false;
    }
}
