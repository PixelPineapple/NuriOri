using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawModeTrigger : MonoBehaviour
{

    private GameObject manager;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("GameManager").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateImage()
    {
        bool drawMode = manager.GetComponent<PlaySystem>().IsDrawMode();
        transform.Find("ModeOnImage").gameObject.SetActive(drawMode);
        transform.Find("ModeOffImage").gameObject.SetActive(!drawMode);
    }
}
