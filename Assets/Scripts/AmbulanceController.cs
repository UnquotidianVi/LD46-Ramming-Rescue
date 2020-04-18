using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmbulanceController : MonoBehaviour
{
    public float Speed { get { return speed; } }
    public bool InputsEnabled { get { return inputsEnabled; } set { inputsEnabled = value; } }

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

    private bool isTransitioningToOtherLane;
    private float laneTransitioningSpeed;
    private bool inputsEnabled = true;

    public void ResetAmbulance()
    {
        currentLaneIndex = startingLaneindex;
        transform.position = new Vector2(highwayManager.Lanes[startingLaneindex].transform.position.x, 0);
        speed = startingSpeed;
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
        transform.Translate(new Vector2(laneTransitioningSpeed, 0) * Time.deltaTime);
        if (Mathf.Abs(transform.position.x - highwayManager.Lanes[currentLaneIndex].transform.position.x) < 0.1f)
        {
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
        if(speedText != null)
            speedText.text = Mathf.RoundToInt( speed ) + "km/h";
    }

    private void CheckForCollisions()
    {
        List<GameObject> frontPickupables = collisonChecker.CheckForCollisions(Vector2.up, "Pickupable", 1f);
        for (int i = 0; i < frontPickupables.Count; i++)
        {
            DroppedItem droppedItem = frontPickupables[i].GetComponent<DroppedItem>();
            if (ambulanceInventoryManager.AddItem(droppedItem.MyItem))
            {
                Destroy(droppedItem.gameObject);
                ambulanceHoweringText.ShowText("You picked up... " + droppedItem.MyItem.name);
            }
            else
            {
                ambulanceHoweringText.ShowText("You can't pickup anymore items...");
            }
        }

        List<GameObject> frontCarCollisions = collisonChecker.CheckForCollisions(Vector2.up, "Car", 1f);
        if (frontCarCollisions.Count >= 1)
            Crash(frontCarCollisions[0].GetComponent<Car>(), Vector2.up);
    }

    private void Crash(Car car, Vector2 direction)
    {
        if(direction == Vector2.up)
        {
            ambulanceHoweringText.ShowText("CRASHED!!! Patient started bleeding more!!!");
            float relativeSpeed = car.Speed / 3.6f - speed / 3.6f;
            car.Speed += -relativeSpeed / 2;
            speed -= -relativeSpeed / 2;
        }
    }
}
