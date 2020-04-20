using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public float XPos { get { return xPos; } set { xPos = value; } }
    public List<Car> Cars { get { return cars; } set { cars = value; } }

    [SerializeField]
    private bool spawnCars;
    [SerializeField]
    private bool spawnItems;

    [SerializeField]
    private float spawnYPos;
    [SerializeField]
    private float carSpawnFrequency;
    [SerializeField]
    private float maxFrequencyVariation;
    [SerializeField]
    private float roadPieceHeight;
    [SerializeField]
    private int numOfRoadPieces;
    [SerializeField]
    private float roadPieceInstantiationYPos;
    [SerializeField]
    private int carLimitOnLane;
    [SerializeField]
    private List<Item> spawnableItems = new List<Item>();
    [SerializeField]
    private float maxItemsSpawnCooldown;
    [SerializeField]
    private float minItemsSpawnCooldown;

    [Header("Object/Prefab References")]
    [SerializeField]
    private GameObject roadPiecePrefab;
    [SerializeField]
    private GameObject carPrefab;
    [SerializeField]
    private GameObject droppedItemPrefab;

    private float lastDistanceCarWasSpawnedAt;
    private float lastTimeItemWasSpawned;
    private float itemSpawnCooldown;
    private float xPos;
    private List<Car> cars = new List<Car>();
    private HighwayManager highwayManager;
    private List<GameObject> roadpieces = new List<GameObject>();
    private GameManager gameManager;

    private void Start()
    {
        highwayManager = FindObjectOfType<HighwayManager>();
        gameManager = FindObjectOfType<GameManager>();
        itemSpawnCooldown = Random.Range(minItemsSpawnCooldown, maxItemsSpawnCooldown);
        if(spawnCars)
            SpawnCar();
        InstantiateRoadPieces();
    }

    private void Update()
    {
        if(gameManager.CurrentGameState == GameManager.GameState.Driving || gameManager.CurrentGameState == GameManager.GameState.Paramedic)
            CheckForSpawnChances();
        if(roadpieces.Count != 0)
        UpdateRoadPieces();
    }

    private void CheckForSpawnChances()
    {
        if (lastDistanceCarWasSpawnedAt - carSpawnFrequency > gameManager.MetersToHospital && spawnCars)
            if (IsSpawnPointFree())
                SpawnCar();
        if (lastTimeItemWasSpawned + itemSpawnCooldown < Time.time && spawnItems)
            SpawnDroppedItem();
    }

    private void SpawnCar()
    {
        Car car = Instantiate(carPrefab).GetComponent<Car>();
        car.transform.position = new Vector2(transform.position.x, spawnYPos);
        car.Speed = highwayManager.AverageSpeed + Random.Range(-highwayManager.SpeedVariation, highwayManager.SpeedVariation);
        Cars.Add(car);
        car.transform.SetParent(gameObject.transform);
        lastDistanceCarWasSpawnedAt = gameManager.MetersToHospital + Random.Range(-maxFrequencyVariation, maxFrequencyVariation);

        if (cars.Count > carLimitOnLane)
        {
            Car carToRemove = cars[0];
            cars.Remove(carToRemove);
            Destroy(carToRemove.gameObject);
        }
    }

    private bool IsSpawnPointFree()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            if (Mathf.Abs(spawnYPos - cars[i].transform.position.y) < 20)
                return false;
        }
        return true;
    }

    private void InstantiateRoadPieces()
    {
        for (int i = 0; i < numOfRoadPieces; i++)
        {
            GameObject roadpiece = Instantiate(roadPiecePrefab);
            roadpiece.transform.SetParent(transform);
            roadpiece.transform.position = new Vector2(transform.position.x, roadPieceInstantiationYPos - roadPieceHeight * i);
            roadpieces.Add(roadpiece);
        }
    }

    private void UpdateRoadPieces()
    {
        for (int i = 0; i < roadpieces.Count; i++)
            roadpieces[i].transform.Translate(new Vector2(0, -highwayManager.ApparentRoadSpeed / 3.6f) * Time.deltaTime);

        GameObject newestRoadpiece = roadpieces[roadpieces.Count - 1];

        if (newestRoadpiece.transform.position.y - roadPieceInstantiationYPos < -roadPieceHeight)
        {
            GameObject oldestRoadpiece = roadpieces[0];
            roadpieces.Remove(oldestRoadpiece);
            oldestRoadpiece.transform.position = new Vector2(transform.position.x, newestRoadpiece.transform.position.y + roadPieceHeight);
            roadpieces.Add(oldestRoadpiece);
        }
    }

    private void SpawnDroppedItem()
    {
        DroppedItem droppedItem = Instantiate(droppedItemPrefab).GetComponent<DroppedItem>();
        droppedItem.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + spawnYPos);
        droppedItem.gameObject.transform.SetParent(transform);
        Item randomItem = spawnableItems[Random.Range(0, spawnableItems.Count)];

        droppedItem.MyItem = randomItem;

        lastTimeItemWasSpawned = Time.time;
        itemSpawnCooldown = Random.Range(minItemsSpawnCooldown, maxItemsSpawnCooldown);
    }
}
