using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour {

    float changeRed = 1.0f;
    float changeGreen = 1.0f;
    float cahngeBlue = 1.0f;
    float cahngeAlpha = 1.0f;

    bool addCheck;
    // このゲームオブジェクトはもうコーヒーされたか？
    bool isDuplicated = false;

    GameObject generator;

    Color[] buffer;

    float pixelCounter;//色のあるピクセル数

    private GameObject manager;

    // Use this for initialization
    void Start () {

       generator = GameObject.Find("ObjectGenerator");

        //addCheck = false;

        Texture2D mainTexture = (Texture2D)gameObject.GetComponent<SpriteRenderer>().sprite.texture;

        Color[] pixels = mainTexture.GetPixels();
        buffer = new Color[pixels.Length];
        pixels.CopyTo(buffer, 0);

        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i].a >= 0.5f)
            {
                pixelCounter++;
            }
        }

        manager = GameObject.Find("GameManager");
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetMouseButtonDown(0))
        {

            //プレイ会に向けて一時的に使用
            bool drawMode = manager.GetComponent<PlaySystem>().IsDrawMode();
            if ( drawMode) return;

            float pixelCheck = 0;
            Texture2D mainTexture = (Texture2D)gameObject.GetComponent<SpriteRenderer>().sprite.texture;

            Color[] pixels = mainTexture.GetPixels();
            buffer = new Color[pixels.Length];
            pixels.CopyTo(buffer, 0);

            if (!addCheck)
            {
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i].r == 0)
                    {
                        pixelCheck++;

                    }
                }
            }

            //黒く塗った割合
            float drawRate = pixelCheck / pixelCounter*100f;

            //一定以上塗っていたら複製
            if(drawRate>80 && addCheck == false)
            {
                addCheck = true;
                
                generator.GetComponent<ObjectGenerator>().AddGameObject(this.gameObject);
            }
        }
       
    }
    

    public void ChangeAddCheck(bool addCheck)
    {
        this.addCheck = addCheck;
    }

    public bool GetAddChaeck()
    {
        return addCheck;
    }
    
    public bool GetIsDuplicated()
    {
        return isDuplicated;
    }

    public void SetIsDuplicated(bool value)
    {
        isDuplicated = value;
    }
}
