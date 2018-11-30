using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{

    public GameObject switchObject;
    public float x, y;
    private float z = 20;

    private Vector3 targetDirection;
	private bool isEnd;

    // Use this for initialization
    void Start()
    {
		isEnd = false;
        targetDirection = transform.position - new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
		if (isEnd) return;

        bool condition = switchObject.GetComponent<PaintBrush>().GetFill();
        if (condition)
        {
            if ((transform.position - new Vector3(x, y, z)).sqrMagnitude > 0.1f)
            {
                transform.position -= targetDirection * Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3(x, y, z);
				isEnd = true;
            }
        }
    }
}
