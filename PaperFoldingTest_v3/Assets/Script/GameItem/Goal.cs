using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    private bool goal;

	// Use this for initialization
	void Start () {
        goal = false;
	}
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if ( other.gameObject.tag != "Hero" ) return;

        if ( ( transform.position - other.transform.position ).sqrMagnitude < 0.03f ) {
            other.GetComponent<Hero>().SetState(CharaState.Stay);
            goal = true;
        }
    }

    public bool IsGoal()
    {
        return goal;
    }
}
