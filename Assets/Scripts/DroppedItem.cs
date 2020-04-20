using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item MyItem { get { return myItem; } set { myItem = value; } }

    [SerializeField]
    private Item myItem;
    [SerializeField]
    private float speed;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameManager gameManager;

    private AmbulanceController ambulanceController;

    private void Start()
    {
        ambulanceController = FindObjectOfType<AmbulanceController>();
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.MetersToHospital > myItem.requiredMetersFromHospital)
            Destroy(gameObject);
        if (myItem.sprite != null)
            spriteRenderer.sprite = myItem.sprite;
    }

    private void Update()
    {
        MoveObject();
    }

    private void MoveObject()
    {
        float relativeSpeedToPlayer = speed / 3.6f - ambulanceController.Speed / 3.6f;
        transform.Translate(new Vector2(0, relativeSpeedToPlayer) * Time.deltaTime);

        if(transform.position.y + 10 < ambulanceController.transform.position.y)
            Destroy(gameObject);
    }

}
