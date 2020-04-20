using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamedicInteract : MonoBehaviour
{
    [SerializeField]
    private CollisionChecker collisionChecker;
    [SerializeField]
    private ParamedicController paramedicController;

    bool inputsEnabledLastFrame;

    private void Update()
    {
        if (paramedicController.InputsEnabled)
        {
            if (inputsEnabledLastFrame)
                CheckForInteractableObjects();
            else
                inputsEnabledLastFrame = true;
        }
        else
        {
            inputsEnabledLastFrame = false;
        }
            
    }

    private void CheckForInteractableObjects()
    {
        List<GameObject> rightInteractableObjects = new List<GameObject>();
        List<GameObject> leftInteractableObjects = new List<GameObject>();

        leftInteractableObjects = collisionChecker.CheckForCollisions(Vector2.left, "Interactable", 0.5f);
        rightInteractableObjects = collisionChecker.CheckForCollisions(Vector2.right, "Interactable", 0.5f);

        if (leftInteractableObjects != null)
            if(leftInteractableObjects.Count > 0)
                InteractWithObject(leftInteractableObjects[0]);

        if (rightInteractableObjects != null)
            if (rightInteractableObjects.Count > 0)
                InteractWithObject(rightInteractableObjects[0]);
    }

    private void InteractWithObject(GameObject go)
    {
        IInteract interactable = go.GetComponent<IInteract>();

        if (interactable != null)
        {
            interactable.AbleToInteract();
            if (Input.GetKeyDown(KeyCode.F))
                interactable.Interact();
        }
    }
}
