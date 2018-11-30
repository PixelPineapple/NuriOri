using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Right = 1,
    Left = -1
}

public enum CharaState
{
    MoveForward,
    MoveUp,
    JumpUp,
    JumpForward,
    Fall,
    Stay,
}

public class Hero : MonoBehaviour
{
    private GameObject manager;

    private Direction dir;
    private CharaState state;
    public float speed;

    public bool[] detector;

    //animation用
    private bool isWalk;
    private bool isClimb;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("GameManager");
        dir = Direction.Right;
        state = CharaState.Fall;
        detector = new bool[6];
        isWalk = false;
        isClimb = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameProgress progress = manager.GetComponent<PlaySystem>().GetProgress();
        if (progress != GameProgress.Play) return;

        UpdateDetector();
        Move();
        UpdateAnimFlag();

        if (transform.position.y < -6.0f) Death();
    }

    private void UpdateDetector()
    {
        for (int i = 0; i <= 5; ++i)
        {
            detector[i] = transform.GetChild(i).GetComponent<Detector>().GetContaction();
        }
    }

    private void AI()
    {
        if (!detector[0])
        {
            //落下途中止まらないためにx座標修正
            float x = (int)(transform.position.x / 0.64f);
            x += (transform.position.x > 0) ? 0.5f : -0.5f;
            transform.position = new Vector3(x * 0.64f, transform.position.y, transform.position.z);

            state = CharaState.Fall;
            isWalk = false;
            isClimb = false;
            return;
        }

        if (detector[3] && detector[5]) ReverseDir();

        if (detector[3] && !detector[5])
        {
            Jump(0.7f, 4.3f);
            state = CharaState.JumpUp;
            isWalk = true;
            isClimb = false;
            return;
        }

        if (!detector[3] && !detector[5] && !detector[1] && detector[2])
        {
            Jump(2.1f * speed, 2.5f);
            state = CharaState.JumpForward;
            isWalk = true;
            isClimb = false;
            return;
        }

        state = CharaState.MoveForward;
        isWalk = true;
        isClimb = false;
    }

    private void Move()
    {
        float y;
        switch (state)
        {
            case CharaState.MoveForward:
                transform.position += new Vector3(Time.deltaTime * speed * (int)dir, 0, 0);
                AI();
                break;
            case CharaState.MoveUp:
                transform.position += new Vector3(0, Time.deltaTime * speed, 0);
                isClimb = true;
                break;
            case CharaState.JumpUp:
                //Debug.Log("JumpUp");
                y = GetComponent<Rigidbody2D>().velocity.y;
                GetComponent<Rigidbody2D>().velocity.Set(0.7f, y);
                if (detector[0]) AI();
                break;
            case CharaState.JumpForward:
                //Debug.Log("JumpForwrd");
                y = GetComponent<Rigidbody2D>().velocity.y;
                if (detector[0]) AI();
                break;
            case CharaState.Fall:
                //Debug.Log("Fall");
                if (detector[0]) AI();
                break;
            case CharaState.Stay:
                isWalk = false;
                isClimb = false;
                break;
        }
    }

    public void Climb(){
        isClimb=true;
        state = CharaState.MoveUp;
    }

    public void Jump(float jumpPowerX, float jumpPowerY)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2((int)dir * jumpPowerX, jumpPowerY);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
    public void SetState(CharaState state)
    {
        this.state = state;
    }
    #region Set Direction
    public void SetDir(Direction dir)
    {
        if (this.dir != dir)
        {
            this.dir = dir;

            //rotate around y axis
            transform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
    public void ReverseDir()
    {
        if (dir == Direction.Right) SetDir(Direction.Left);
        else SetDir(Direction.Right);
    }
    #endregion
    #region Animation
    private void UpdateAnimFlag()
    {
        GetComponent<Animator>().SetBool("walk", isWalk);
        GetComponent<Animator>().SetBool("climb", isClimb);
    }
    #endregion
}
