using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeButton : MonoBehaviour
{
    bool drawMode;
    float time;

    // Use this for initialization
    void Start()
    {
        drawMode = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnClick()
    {
        drawMode = !drawMode;
        Debug.Log("mode=" + drawMode);
    }

    public bool IsDrawMode()
    {
        return drawMode;    
    }
}
