using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag != "Hero") return;

        other.gameObject.GetComponent<Hero>().speed *= 2;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.tag != "Hero") return;

        other.gameObject.GetComponent<Hero>().speed /= 2;
    }
}
