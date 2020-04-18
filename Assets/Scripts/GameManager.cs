using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Driving, Paramedic, Death }
    public GameState CurrentGameState { get { return currentGameState; }  }

    private GameState currentGameState;
    [SerializeField]
    private GameState startingGameState;
    [SerializeField]
    private Camera drivingCamera;
    [SerializeField]
    private Camera paramedicCamera;
    [SerializeField]
    private AmbulanceController ambulanceController;
    [SerializeField]
    private ParamedicController paramedicController;
    [SerializeField]
    private GameObject drivingUI;
    [SerializeField]
    private GameObject paramedicUI;
    [SerializeField]
    private GameObject deathUI;
    [SerializeField]
    private HighwayManager highwayManager;
    [SerializeField]
    private PatientBehaviour patientBehaviour;
    [SerializeField]
    private Seat seat;

    private void Start()
    {
        StartNewGame();
    }

    private void Update()
    {
        CheckForInputs();
    }

    public void ChangeGameState(GameState gameStateToChangeTo)
    {
        currentGameState = gameStateToChangeTo;

        ambulanceController.InputsEnabled = false;
        paramedicController.InputsEnabled = false;

        drivingCamera.enabled = false;
        paramedicCamera.enabled = false;

        if (drivingUI != null)
            drivingUI.SetActive(false);
        if (paramedicUI != null)
            paramedicUI.SetActive(false);
        if (deathUI != null)
            deathUI.SetActive(false);

        switch (gameStateToChangeTo)
        {
            case GameState.Driving:
                ambulanceController.InputsEnabled = true;
                drivingCamera.enabled = true;
                if(drivingUI != null)
                    drivingUI.SetActive(true);
                break;
            case GameState.Paramedic:
                paramedicController.InputsEnabled = true;
                paramedicCamera.enabled = true;
                if (paramedicUI != null)
                    paramedicUI.SetActive(true);
                paramedicController.transform.position = new Vector2(seat.transform.position.x + 1.5f, 0);
                break;
            case GameState.Death:
                ambulanceController.InputsEnabled = true;
                drivingCamera.enabled = true;
                if (deathUI != null)
                    deathUI.SetActive(true);
                break;
        }
    }

    private void CheckForInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.Driving)
                ChangeGameState(GameState.Paramedic);
        }

        if(currentGameState == GameState.Death)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartNewGame();
        }
    }

    private void StartNewGame()
    {
        highwayManager.WipeLevel();
        highwayManager.LoadLevel();
        patientBehaviour.ResetPatient();
        ambulanceController.ResetAmbulance();
        ChangeGameState(GameState.Driving);
    }
}
