using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwayManager : MonoBehaviour
{
    public List<Lane> Lanes { get { return lanes; } }
    public float AverageSpeed { get { return averageSpeed; } }
    public float SpeedVariation { get { return speedVariation; } }
    public float ApparentRoadSpeed { get { return apparentRoadSpeed; } set { apparentRoadSpeed = value; } }

    [SerializeField]
    private float averageSpeed;
    [SerializeField]
    private float speedVariation;
    [SerializeField]
    private int numOfLanes;
    [SerializeField]
    private float laneSeparation;
    [SerializeField]
    private float backgroundLaneSperation;
    [SerializeField]
    private float firsLanePosition;
    [SerializeField]
    private List<Lane> lanes = new List<Lane>();
    [SerializeField]
    private GameObject lanePrefab;
    [SerializeField]
    private GameObject laneParent;
    [SerializeField]
    private GameObject backgroundLanePrefab;
    [SerializeField]
    private List<Lane> backgroundLanes = new List<Lane>();

    private float apparentRoadSpeed;

    public void LoadLevel()
    {
        InstantiateLanes();
    }

    private void InstantiateLanes()
    {
        Lane backgroundLane = InstantiateBackgroundLane();
        backgroundLane.XPos = firsLanePosition - backgroundLaneSperation;
        backgroundLane.gameObject.transform.position = new Vector2(backgroundLane.XPos, 0);

        for (int i = 0; i < numOfLanes; i++)
        {
            Lane lane = InstantiateLane();
            lane.XPos = firsLanePosition + laneSeparation * i;
            lane.gameObject.transform.position = new Vector2(lane.XPos, 0);
            
        }

        backgroundLane = InstantiateBackgroundLane();
        backgroundLane.XPos = lanes[lanes.Count - 1].transform.position.x + backgroundLaneSperation;
        backgroundLane.gameObject.transform.position = new Vector2(backgroundLane.XPos, 0);
    }

    public void WipeLevel()
    {
        for (int i = 0; i < lanes.Count; i++)
        {
            Lane lane = lanes[i];
            lanes.Remove(lane);
            Destroy(lane.gameObject);
            i--;
        }
    }

    private Lane InstantiateLane()
    {
        Lane lane = Instantiate(lanePrefab).GetComponent<Lane>();
        Lanes.Add(lane);
        lane.transform.SetParent(laneParent.transform);
        return lane;
    }

    private Lane InstantiateBackgroundLane()
    {
        Lane lane = Instantiate(backgroundLanePrefab).GetComponent<Lane>();
        lane.transform.SetParent(laneParent.transform);
        backgroundLanes.Add(lane);
        return lane;
    }
}
