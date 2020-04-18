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
    private float firsLanePosition;
    [SerializeField]
    private List<Lane> lanes = new List<Lane>();
    [SerializeField]
    private GameObject lanePrefab;
    [SerializeField]
    private GameObject laneParent;

    private float apparentRoadSpeed;

    public void LoadLevel()
    {
        InstantiateLanes();
    }

    private void InstantiateLanes()
    {
        for (int i = 0; i < numOfLanes; i++)
        {
            Lane lane = Instantiate(lanePrefab).GetComponent<Lane>();
            lane.XPos = firsLanePosition + laneSeparation * i;
            lane.gameObject.transform.position = new Vector2(lane.XPos, 0);
            Lanes.Add(lane);
            lane.transform.SetParent(laneParent.transform);
        }
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
}
