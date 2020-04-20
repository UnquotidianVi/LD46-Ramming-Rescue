using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { Driving, Paramedic, Death, Win, MainMenu }
    public GameState CurrentGameState { get { return currentGameState; }  }
    public Text DeathUIDescriptionText { get { return deathUIDescriptionText; } set { deathUIDescriptionText = value; } }
    public float MetersToHospital { get { return metersToHospital; } }
    public bool GamePausedOnTheBackground { get { return gamePausedOnTheBackground; } }

    [SerializeField]
    private bool openMainMenuAtStart;
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
    private GameObject winUI;
    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private HighwayManager highwayManager;
    [SerializeField]
    private PatientBehaviour patientBehaviour;
    [SerializeField]
    private Seat seat;
    [SerializeField]
    private AmbulanceInventoryManager ambulanceInventoryManager;
    [SerializeField]
    private float metersToHospital;
    [SerializeField]
    private Text distanceToHospitalText;
    [SerializeField]
    private Text deathUIDescriptionText;
    [SerializeField]
    private Sound winSound;
    [SerializeField]
    private SoundManager soundManager;

    float startingMetersToHospital;
    private GameState currentGameState;
    float timeChangedtDrivingMode;
    private bool gamePausedOnTheBackground;

    private void Start()
    {
        startingMetersToHospital = metersToHospital;
        if (openMainMenuAtStart)
            ChangeGameState(GameState.MainMenu);
        else
            StartNewGame();
    }

    private void Update()
    {
        CheckForInputs();
        if(currentGameState != GameState.Death && currentGameState != GameState.Win)
            metersToHospital -= ambulanceController.Speed / 3.6f * Time.deltaTime;
        if (metersToHospital <= 0 && currentGameState != GameState.Win)
            ChangeGameState(GameState.Win);
        UpdateUI();
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
        if (winUI != null)
            winUI.SetActive(false);
        if (mainMenuUI != null)
            mainMenuUI.SetActive(false);

        ambulanceController.CanPickupItems = false;

        switch (gameStateToChangeTo)
        {
            case GameState.Driving:
                ambulanceController.InputsEnabled = true;
                drivingCamera.enabled = true;
                if(drivingUI != null)
                    drivingUI.SetActive(true);
                ambulanceController.CanPickupItems = true;
                timeChangedtDrivingMode = Time.time;
                break;
            case GameState.Paramedic:
                paramedicController.InputsEnabled = true;
                paramedicCamera.enabled = true;
                if (paramedicUI != null)
                    paramedicUI.SetActive(true);
                paramedicController.transform.position = new Vector2(seat.transform.position.x + 0.5f, paramedicController.transform.position.y);
                break;
            case GameState.Death:
                ambulanceController.InputsEnabled = true;
                drivingCamera.enabled = true;
                if (deathUI != null)
                    deathUI.SetActive(true);
                break;
            case GameState.Win:
                drivingCamera.enabled = true;
                soundManager.PlaySound(winSound);
                if(winUI != null)
                    winUI.SetActive(true);
                break;
            case GameState.MainMenu:
                drivingCamera.enabled = true;
                if (mainMenuUI != null)
                    mainMenuUI.SetActive(true);
                break;
        }
    }

    private void CheckForInputs()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentGameState == GameState.Driving && timeChangedtDrivingMode + 0.1f < Time.time)
                ChangeGameState(GameState.Paramedic);
        }

        if(currentGameState == GameState.Death || currentGameState == GameState.Win)
        {
            if (Input.GetKeyDown(KeyCode.R))
                StartNewGame();
        }

        if(currentGameState == GameState.Driving || currentGameState == GameState.Paramedic)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
    }

    public void StartNewGame()
    {
        metersToHospital = startingMetersToHospital;
        highwayManager.WipeLevel();
        highwayManager.LoadLevel();
        patientBehaviour.ResetPatient();
        ambulanceController.ResetAmbulance();
        ambulanceInventoryManager.WipeInventory();
        ChangeGameState(GameState.Driving);
        Time.timeScale = 1;
        gamePausedOnTheBackground = false;
    }

    public void PauseGame()
    {
        gamePausedOnTheBackground = true;
        Time.timeScale = 0;
        ChangeGameState(GameState.MainMenu);
    }

    public void ContinueGame()
    {
        gamePausedOnTheBackground = false;
        Time.timeScale = 1;
        ChangeGameState(GameState.Driving);
    }

    private void UpdateUI()
    {
        if (metersToHospital >= 1000)
            distanceToHospitalText.text = "Hospital: " + (metersToHospital / 1000).ToString("F1") + "km";
        else
            distanceToHospitalText.text = "Hospital: " + Mathf.RoundToInt(metersToHospital) + "m";
    }
}
