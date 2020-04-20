using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamedicController : MonoBehaviour
{
    public bool InputsEnabled { get { return inputsEnabled; } set { inputsEnabled = value; } }
    public Vector2 FacingTowards { get { return facingTowards; } set { facingTowards = value; } }

    [SerializeField]
    private CollisionChecker collisionChecker;

    private bool inputsEnabled = false;
    [SerializeField]
    private float speed;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;

    private bool isAbleToMoveLeft;
    private bool isAbleToMoveRight;
    private Vector2 facingTowards;
    private bool isWalking;

    private void Update()
    {
        CheckForCollisions();
        if (inputsEnabled)
            CheckForInputs();
        UpdateVisuals();
    }

    private void CheckForInputs()
    {
        isWalking = true;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if(isAbleToMoveLeft)
                Move(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if(isAbleToMoveRight)
                Move(Vector2.right);
        }
        else
        {
            isWalking = false;
        }
    }

    private void Move(Vector2 direction)
    {
        facingTowards = direction;
        transform.Translate(direction * speed * Time.deltaTime);
    } 

    private void CheckForCollisions()
    {
        List<GameObject> leftCollisions = collisionChecker.CheckForCollisions(Vector2.left, "Wall", 0.5f);
        List<GameObject> rightCollisions = collisionChecker.CheckForCollisions(Vector2.right, "Wall", 0.5f);

        if (leftCollisions.Count >= 1)
            isAbleToMoveLeft = false;
        else
            isAbleToMoveLeft = true;

        if (rightCollisions.Count >= 1)
            isAbleToMoveRight = false;
        else
            isAbleToMoveRight = true;
    }

    private void UpdateVisuals()
    {
        if (facingTowards == Vector2.left)
            spriteRenderer.flipX = true;
        else if(facingTowards == Vector2.right)
            spriteRenderer.flipX = false;

        animator.SetBool("IsWalking", isWalking);
    }
}
