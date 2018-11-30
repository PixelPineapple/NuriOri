using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLadderDetector : MonoBehaviour {

    private bool contaction;

    // Use this for initialization
    void Start()
    {
        contaction = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Ladder") return;
        contaction = true;
    }

    void OnTriggerExit(Collider other)
    {
        contaction = false;
    }

    public bool GetContaction()
    {
        return contaction;
    }
}
