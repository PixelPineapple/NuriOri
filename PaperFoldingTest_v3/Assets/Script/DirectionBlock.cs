using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class DirectionBlock : MonoBehaviour {

    public LayerMask collisionMask;
    public Direction direction;
    bool isSteppedOn = false;

    public void Update()
    {
        Vector2 rayOrigin = GetComponent<BoxCollider2D>().bounds.center;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, 0.8f, collisionMask);
        Debug.DrawRay(rayOrigin, Vector2.up, Color.red);

        if(hit)
        {
            ChangeCharacterDirection(hit);
        }
    }

    void ChangeCharacterDirection(RaycastHit2D hit)
    {
        if (isSteppedOn) return;

        Debug.Log("test");
        hit.transform.GetComponent<Hero>().SetDir(direction);
        isSteppedOn = true;
    }

    public void FlipDirection()
    {
        if (direction == Direction.Left) direction = Direction.Right;
        else direction = Direction.Left;

        transform.localScale = new Vector3((float)direction, 1, 1);
    }


}
