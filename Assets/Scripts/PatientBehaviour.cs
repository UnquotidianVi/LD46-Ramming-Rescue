using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientBehaviour : MonoBehaviour, IInteract
{

    public float Blood { get { return blood; } set { blood = value; } }
    public float BloodLoss { get { return bloodLoss; } set { bloodLoss = value; } }
    public float Awaraness { get { return awareness; } set { awareness = value; } }

    [SerializeField]
    private Image awaranessMeter;
    [SerializeField]
    private Image bloodMeter;
    [SerializeField]
    private Text bloodLossText;
    [SerializeField]
    private Text awaranessLossText;
    [SerializeField]
    private GameManager gameManager;

    private float blood;
    private float bloodLoss;
    private float awaranessLoss;
    private float awareness;

    public void ResetPatient()
    {
        awareness = 10f;
        blood = 10f;
        bloodLoss = 0.01f;
    }

    private void Update()
    {
        CalculatePatientStats();
        UpdatePatientUI();
    }

    public void Interact()
    {

    }

    public void AbleToInteract()
    {

    }

    private void CalculatePatientStats()
    {
        if (blood > 10)
            blood = 10f;
        if (awareness > 10)
            awareness = 10f;
        if (bloodLoss < 0)
            bloodLoss = 0;

        blood -= bloodLoss * Time.deltaTime;
        awaranessLoss = 1f - blood / 10;
        awareness -= awaranessLoss * Time.deltaTime;

        if (awareness <= 0 || blood <= 0)
            gameManager.ChangeGameState(GameManager.GameState.Death);
    }

    private void UpdatePatientUI()
    {
        awaranessMeter.fillAmount = awareness / 10;
        bloodMeter.fillAmount = blood / 10;
        bloodLossText.text = bloodLoss.ToString();
        awaranessLossText.text = awaranessLoss.ToString();
    }
}
