using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private bool contaction;
    int count;

    // Use this for initialization
    void Start()
    {
        contaction = false;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ladder") return;
        if (collision.tag == "Enemy") return;
        if (collision.tag == "Goal") return;
        if (collision.tag == "Uncollidable") return;
        count++;
        //Debug.Log(collision.name.ToString());
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ladder") return;
        if (collision.tag == "Enemy") return;
        if (collision.tag == "Goal") return;
        if (collision.tag == "Uncollidable") return;
        contaction = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        count--;
        if (count < 0) count = 0;
        
        if (count==0) contaction = false;

    }
    public bool GetContaction()
    {
        return contaction;
    }
}
