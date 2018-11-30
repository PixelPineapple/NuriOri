using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    bool climb;
    GameObject topCollider;

    // Use this for initialization
    void Start()
    {
        climb = false;
        topCollider = transform.Find("TopCollider").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Hero") return;
        if (GetComponent<Collider2D>().bounds.max.y > other.transform.position.y + 0.3f)
        {
            //上の当たり判定追加OFF
            topCollider.SetActive(false);
            climb = true;

            //heroを梯子の中心に移動する
            other.transform.position = new Vector3(GetComponent<Collider2D>().bounds.center.x, other.transform.position.y, 20);
            other.GetComponent<Hero>().SetState(CharaState.MoveUp);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag != "Hero") return;
        if (!climb) return;

        other.GetComponent<Rigidbody2D>().gravityScale = 0;
        other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Hero") return;
        if (!climb) return;
        climb = false;
        other.GetComponent<Rigidbody2D>().gravityScale = 1;
        other.GetComponent<Hero>().SetState(CharaState.MoveForward);

        //上の当たり判定追加ON
        topCollider.SetActive(true);
    }
}