using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour {

	public int index;
	// Use this for initialization
	void Start () {
	}

	void OnTriggerStay2D(Collider2D other)
	{
		GetComponentInParent<Water>().SetEmpty(index,false);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		GetComponentInParent<Water>().SetEmpty(index,true);
	}
}
