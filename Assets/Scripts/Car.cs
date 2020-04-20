using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float Speed { get { return speed; } set { speed = value; } }
    private float speed;


    [SerializeField]
    private CollisionChecker collisionChecker;
    [SerializeField]
    private List<Sprite> carSprites = new List<Sprite>();
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private HighwayManager highwayManager;
    private GameManager gameManager;
    private AmbulanceController ambulanceController;

    private void Start()
    {
        highwayManager = FindObjectOfType<HighwayManager>();
        ambulanceController = FindObjectOfType<AmbulanceController>();
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer.sprite = carSprites[Random.Range(0, carSprites.Count)];
    }

    private void Update()
    {
        UpdatePosition();
        CheckCollisions();

        if (gameManager.CurrentGameState == GameManager.GameState.Win)
            transform.Translate(new Vector2(0, 3 * Time.deltaTime));
    }

    private void UpdatePosition()
    {
        float relativeSpeedToPlayer = speed / 3.6f - ambulanceController.Speed / 3.6f;
        transform.Translate(new Vector2(0, relativeSpeedToPlayer) * Time.deltaTime);
    }

    private void CheckCollisions()
    {
        List<GameObject> frontCollisions = collisionChecker.CheckForCollisions(Vector2.up, "Car", 2f);

        for (int i = 0; i < frontCollisions.Count; i++)
            if(frontCollisions[i] != this.gameObject)
            {
                Car otherCar = frontCollisions[i].GetComponent<Car>();
                if (otherCar.Speed < speed)
                    otherCar.Speed = speed;
            }
                

        frontCollisions = collisionChecker.CheckForCollisions(Vector2.up, "Player", 2f);

        for (int i = 0; i < frontCollisions.Count; i++)
        {
            AmbulanceController ambulanceController = frontCollisions[i].GetComponent<AmbulanceController>();
            if (ambulanceController.Speed < speed)
                speed = ambulanceController.Speed;
        }
                
    }
}
