using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLaunCher : MonoBehaviour {

    public GameObject arrows;

    float timer;
    

    //矢を放つ方向
    enum Direction
    {
        Right = 0,
        Left = 180
    }
    [SerializeField]
    Direction direction;
    [SerializeField]
    float interval;

    // Use this for initialization
    void Start () {
        timer = 0;
        transform.rotation = new Quaternion(0.0f, 0.0f, (int)direction, 0.0f);

    }
	
	// Update is called once per frame
	void Update () {
        ShootArrows();
	}

    void ShootArrows()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            timer = 0;
            GameObject arrow = Instantiate(arrows) as GameObject;
            switch (direction)
            {
                case Direction.Left:
                    arrow.GetComponent<Arrow>().SetArrowDirection(ArrowDirection.Left);
                    arrow.transform.position = this.transform.position;
                    arrow.transform.rotation = new Quaternion(0.0f, 0.0f, (int)direction, 0.0f);

                    break;
                case Direction.Right:
                    arrow.GetComponent<Arrow>().SetArrowDirection(ArrowDirection.Right);
                    arrow.transform.position = this.transform.position;
                    arrow.transform.rotation = new Quaternion(0.0f, 0.0f, (int)direction, 0.0f);

                    break;

            }
            
        }
    }

    public void ChangeDirection()
    {
        if (direction == Direction.Left) direction = Direction.Right;
        else if (direction == Direction.Right) direction = Direction.Left;

        transform.rotation = new Quaternion(0.0f, 0.0f, (int)direction, 0.0f);
    }
}
