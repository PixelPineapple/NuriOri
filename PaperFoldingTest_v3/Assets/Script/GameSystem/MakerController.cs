using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakerController : MonoBehaviour {

    //色を塗って複製できるようになるまで一時的に使うα版用のスクリプトです

    float changeRed = 1.0f;
    float changeGreen = 1.0f;
    float changeBlue = 1.0f;
    float changeAlpha = 1.0f;

    bool changeColor;



    // Use this for initialization
    void Start()
    {
        changeColor = false;
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの位置をPositionに
        Vector2 aTapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        
        //左クリックしている間maker移動
        if (Input.GetMouseButton(0))
        {
            transform.position = aTapPoint;
        }
        
    }

    
    //衝突したオブジェクトの色を変える
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.tag == "Hero" ) return;
        collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, changeAlpha);
        Debug.Log("通ったyo"+collision.gameObject.ToString());
    }

}
