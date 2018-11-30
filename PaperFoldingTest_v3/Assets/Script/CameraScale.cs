using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour {


	// Use this for initialization
	void Awake () {
		float newSize = (float)Screen.height/(float)Screen.width * 10/16 * 8.2f;
		GetComponent<Camera>().orthographicSize = Mathf.Max(newSize,8.2f);		
	}
}
