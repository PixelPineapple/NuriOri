using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero") other.GetComponent<Hero>().Death();
        if (other.gameObject.tag == "Rock")
        {
            other.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.transform.position = transform.position + new Vector3(0, 0.64f, 0);
        }
    }

    /*
    void OnTriggerExit(Collider2D other)
    {
        if ( other.gameObject.tag == "Rock" ) other.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
    }
    */
}
