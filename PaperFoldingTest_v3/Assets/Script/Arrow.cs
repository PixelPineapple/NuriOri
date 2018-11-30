using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//矢の射出方向
public enum ArrowDirection
{
    Right=1,
    Left=-1,
}

public class Arrow : MonoBehaviour {

    ArrowDirection arrowDir;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Move();
        Death();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Entering Collision : " + collision.gameObject.ToString());
        if (collision.CompareTag("ArrowLauncher")) return;
        if (collision.transform.name.Contains("Detector")) return;
        if (collision.CompareTag("Hero"))
        {
            collision.gameObject.GetComponent<Hero>().Death();
        }
        if (collision.CompareTag("ground")) Destroy(gameObject);
    }

    void Death()
    {
        if (transform.position.x > 15
            || transform.position.x < -15)
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.position += new Vector3(0.3f, 0, 0)*(int)arrowDir;
    }

    public void SetArrowDirection(ArrowDirection arrowDir)
    {
        this.arrowDir = arrowDir;
    }
}
