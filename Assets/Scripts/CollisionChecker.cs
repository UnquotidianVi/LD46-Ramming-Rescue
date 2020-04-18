using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private List<GameObject> ShootRay(Vector2 direction, float length, Vector3 offset, string tag)
    {
        Vector3 origin = transform.position + offset;
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, length);
        List<GameObject> gameObjectshit = new List<GameObject>();

        for (int i = 0; i < hits.Length; i++)
        {
            if(hits[i].collider != null && hits[i].transform.tag == tag)
            {
                Debug.DrawRay(origin, direction, Color.red);
                gameObjectshit.Add(hits[i].transform.gameObject);
            }
        }

        Debug.DrawRay(origin, direction, Color.white);
        return gameObjectshit;
    }

    public List<GameObject> CheckForCollisions(Vector2 direction, string tag, float rayLength)
    {
        Vector3 rayOffset = Vector2.zero;
        if (ShootRay(direction, rayLength, rayOffset, tag) != null)
            return ShootRay(direction, rayLength, rayOffset, tag);

        return new List<GameObject>();
    }
}
