using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDoubleClick : MonoBehaviour
{
    private bool oneClick = false;
    private bool timerRunning;
    private float timerForDoubleClick;
    private float delay = 0.8f;
    private GameObject manager;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("GameManager").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //click
        if ( Input.GetMouseButtonDown(0) ) {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 heroPosition = Camera.main.WorldToScreenPoint(transform.position);
            heroPosition.z = 0;
            if ( ( mousePosition - heroPosition ).sqrMagnitude < 3000.0f ) {
                if ( !oneClick ) {
                    oneClick = true;
                    timerForDoubleClick = Time.time;
                } else {
                    oneClick = false;
                    manager.GetComponent<PlaySystem>().HeroPause();
                }
            }
        }
        //time pass
        if ( oneClick ) {
            if ( Time.time - timerForDoubleClick > delay ) {
                oneClick = false;
            }
        }
    }

}
