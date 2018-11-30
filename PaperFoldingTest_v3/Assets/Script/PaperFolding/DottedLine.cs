using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour {

    private LineRenderer line;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update () {
        var line = gameObject.GetComponent<LineRenderer>();
        var distance = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
        line.materials[0].mainTextureScale = new Vector2(distance, 1f);
	}
}
