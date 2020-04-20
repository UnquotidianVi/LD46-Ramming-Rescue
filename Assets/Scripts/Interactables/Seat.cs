using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour, IInteract
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private HoweringText paramedicHowerableText;

    public void Interact()
    {
        gameManager.ChangeGameState(GameManager.GameState.Driving);
    }

    public void AbleToInteract()
    {
        paramedicHowerableText.ShowText("Press [F] to sit down on drivers seat.");
    }
}
