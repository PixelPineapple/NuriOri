using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {

    RaycastHit hit;
    Ray ray;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            hit = new RaycastHit();
        }
    }

    public RaycastHit GetHit()
    {
        return hit;
    }
    public Ray GetRay()
    {
        return ray;
    }
}
