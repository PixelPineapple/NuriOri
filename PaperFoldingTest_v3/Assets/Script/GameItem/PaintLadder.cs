using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLadder : MonoBehaviour {

    public bool[] detector;
    private GameObject manager;

    // Use this for initialization
    void Start()
    {
        detector = new bool[2];
        manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        GameProgress progress = manager.GetComponent<PlaySystem>().GetProgress();
        if (progress != GameProgress.Play) return;

        UpdateDetector();
    }

    private void UpdateDetector()
    {
        for (int i = 0; i <= 1; ++i)
        {
            detector[i] = transform.GetChild(i).GetComponent<PaintLadderDetector>().GetContaction();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag != "Hero") return;

        other.GetComponent<Rigidbody2D>().gravityScale = 0;
        other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //下からx座標が近い時
        if (GetComponent<Collider2D>().bounds.center.x - other.transform.position.x < 0.1f && GetComponent<Collider2D>().bounds.center.y > other.transform.position.y)
        {
            //heroを梯子の中心に移動する
            other.transform.position = new Vector3(GetComponent<Collider2D>().bounds.center.x, other.transform.position.y, other.transform.position.z);
            other.GetComponent<Hero>().SetState(CharaState.MoveUp);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Hero") return;
        if (detector[0]) return;
        other.GetComponent<Rigidbody2D>().gravityScale = 1;
        other.GetComponent<Hero>().SetState(CharaState.JumpUp);
        other.GetComponent<Hero>().Jump(1.5f, 2.0f);
    }
}
