using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class AmbulanceController : MonoBehaviour
{
    public float Speed { get { return speed; } }
    public bool InputsEnabled { get { return inputsEnabled; } set { inputsEnabled = value; } }
    public bool CanPickupItems { get { return canPickupItems; } set { canPickupItems = value; } }

    [SerializeField]
    private bool ANTICRASH_CHEAT_TOGGLE;

    [SerializeField]
    private int currentLaneIndex;
    [SerializeField]
    private int startingLaneindex;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float startingSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float deacceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float laneTransitionTime;
    [SerializeField]
    private float bloodLossOnHitMultiplier;
    [SerializeField]
    private Sound carCrash;
    [SerializeField]
    private Sound itemPickupSound;
    [SerializeField]
    private float sideCrashBloodLoss;
    [SerializeField]
    private Sprite ambulanceSprite;
    [SerializeField]
    private Sprite explodedSprite;
    [SerializeField]
    private Light2D explosionLight;
    [SerializeField]
    private GameObject lights;
    [SerializeField]
    private float explosionSpriteAliveTime;
    [SerializeField]
    private GameObject speedoMeterNeedle;
    [SerializeField]
    private float lowestRotation;
    [SerializeField]
    private float highestrotation;

    [Header("Script References")]
    [SerializeField]
    private Text speedText;
    [SerializeField]
    private HighwayManager highwayManager;
    [SerializeField]
    private CollisionChecker collisonChecker;
    [SerializeField]
    private AmbulanceInventoryManager ambulanceInventoryManager;
    [SerializeField]
    private HoweringText ambulanceHoweringText;
    [SerializeField]
    private PatientBehaviour patientBehaviour;
    [SerializeField]
    private SoundManager soundManager;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameManager gameManager;

    private bool isTransitioningToOtherLane;
    private float laneTransitioningSpeed;
    private bool inputsEnabled = true;
    private bool canPickupItems;
    private float timeExplosionStarted;

    public void ResetAmbulance()
    {
        currentLaneIndex = startingLaneindex;
        transform.position = new Vector2(highwayManager.Lanes[startingLaneindex].transform.position.x, 0);
        speed = startingSpeed;

        spriteRenderer.sprite = ambulanceSprite;
        explosionLight.intensity = 0;
        lights.SetActive(true);
    }

    private void Update()
    {
        if(inputsEnabled)
            CheckForInputs();
        if (isTransitioningToOtherLane)
            TransitionToCurrentLane();
        UpdateUI();
        CheckForCollisions();
        highwayManager.ApparentRoadSpeed = speed;

        if (gameManager.CurrentGameState == GameManager.GameState.Win)
            transform.Translate(new Vector2(0, 3 * Time.deltaTime));

        if (spriteRenderer.sprite == explodedSprite && timeExplosionStarted + explosionSpriteAliveTime < Time.time)
        {
            spriteRenderer.sprite = null;
            explosionLight.intensity = 0;
        }
            
    }

    private void CheckForInputs()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            ChangeSpeed(acceleration * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            ChangeSpeed(-deacceleration * Time.deltaTime);
        }
    }

    private void ChangeLane(int direction)
    {
        if(currentLaneIndex + direction >= 0 && currentLaneIndex + direction < highwayManager.Lanes.Count)
        {
            currentLaneIndex += direction;
            Lane laneToMoveTo = highwayManager.Lanes[currentLaneIndex];
            laneTransitioningSpeed = (laneToMoveTo.transform.position.x - transform.position.x) / laneTransitionTime;
            isTransitioningToOtherLane = true;
        }  
    }

    private void TransitionToCurrentLane()
    {
        if (laneTransitioningSpeed < 0)
            spriteRenderer.transform.rotation = new Quaternion(0, 0, 0.05f, 1);
        else
            spriteRenderer.transform.rotation = new Quaternion(0, 0, -0.05f, 1);
        transform.Translate(new Vector2(laneTransitioningSpeed, 0) * Time.deltaTime);
        if (Mathf.Abs(transform.position.x - highwayManager.Lanes[currentLaneIndex].transform.position.x) < 0.1f)
        {
            spriteRenderer.transform.rotation = new Quaternion(0, 0, 0, 1);
            isTransitioningToOtherLane = false;
            transform.position = new Vector2(highwayManager.Lanes[currentLaneIndex].transform.position.x, 0);
        }  
    }

    private void ChangeSpeed(float speedChange)
    {
        if(speed + speedChange < maxSpeed && speed + speedChange > minSpeed)
            speed = speed + speedChange;
    }

    private void UpdateUI()
    {
        speedText.text = Mathf.RoundToInt( speed ) + "km/h";

        float numOfNeedleSteps = maxSpeed - minSpeed;
        float needleStepSize = (highestrotation - lowestRotation) / numOfNeedleSteps;
        float currentNeedleStep = speed - minSpeed;

        speedoMeterNeedle.transform.rotation = new Quaternion(0, 0, currentNeedleStep * needleStepSize, 1);
    }

    private void CheckForCollisions()
    {
        if (canPickupItems)
        {
            List<GameObject> frontPickupables = collisonChecker.CheckForCollisions(Vector2.up, "Pickupable", 1f);
            for (int i = 0; i < frontPickupables.Count; i++)
            {
                DroppedItem droppedItem = frontPickupables[i].GetComponent<DroppedItem>();
                if (ambulanceInventoryManager.AddItem(droppedItem.MyItem))
                {
                    Destroy(droppedItem.gameObject);
                    ambulanceHoweringText.ShowText("You picked up... " + droppedItem.MyItem.name);
                    soundManager.PlaySound(itemPickupSound);

                }
                else
                {
                    ambulanceHoweringText.ShowText("You can't pickup anymore items...");
                }
            }
        }

        List<GameObject> frontCarCollisions = collisonChecker.CheckForCollisions(Vector2.up, "Car", 0.7f);
        if (frontCarCollisions.Count >= 1)
            Crash(frontCarCollisions[0].GetComponent<Car>(), Vector2.up);

        List<GameObject> leftCarCollisions = collisonChecker.CheckForCollisions(Vector2.left, "Car", 0.2f);
        if (leftCarCollisions.Count > 0)
            if (isTransitioningToOtherLane && laneTransitioningSpeed < 0)
            Crash(leftCarCollisions[0].GetComponent<Car>(), Vector2.left);

        List<GameObject> rightCarCollisions = collisonChecker.CheckForCollisions(Vector2.right, "Car", 0.2f);
        if(rightCarCollisions.Count > 0)
            if (isTransitioningToOtherLane && laneTransitioningSpeed > 0)
                Crash(rightCarCollisions[0].GetComponent<Car>(), Vector2.right);
    }

    private void Crash(Car car, Vector2 direction)
    {
        if (ANTICRASH_CHEAT_TOGGLE || gameManager.CurrentGameState == GameManager.GameState.Win || ambulanceSprite == null)
            return;

        ambulanceHoweringText.ShowText("CRASHED!!! Patient started bleeding more!!!");

        if (direction == Vector2.up)
        {
            float relativeSpeed = car.Speed - speed;
            car.Speed -= relativeSpeed / 3;
            speed += relativeSpeed;
            car.transform.position = new Vector2(car.transform.position.x, car.transform.position.y + 0.2f);
            patientBehaviour.BloodLoss += -relativeSpeed * bloodLossOnHitMultiplier;
            soundManager.PlaySound(carCrash);
            Camera.main.GetComponent<CameraShake>().ShakeCamera(0.2f, 0.2f);
        }

        if(direction == Vector2.left)
        {
            patientBehaviour.BloodLoss += sideCrashBloodLoss;
            soundManager.PlaySound(carCrash);
            ChangeLane(1);
            Camera.main.GetComponent<CameraShake>().ShakeCamera(0.2f, 0.2f);
        }

        if (direction == Vector2.right)
        {
            patientBehaviour.BloodLoss += sideCrashBloodLoss;
            soundManager.PlaySound(carCrash);
            ChangeLane(-1);
            Camera.main.GetComponent<CameraShake>().ShakeCamera(0.2f, 0.2f);
        }
    }

    public void BlowUp()
    {
        spriteRenderer.sprite = explodedSprite;
        explosionLight.intensity = 1;
        lights.SetActive(false);
        timeExplosionStarted = Time.time;
    }
}
