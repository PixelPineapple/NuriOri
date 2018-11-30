using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour {
    
    Texture2D drawTexture;
    Color[] buffer;
    bool touching = false;
    Vector2 prevPoint;

    Sprite sprite;
    string objectName;

    //GameObject maker;
    private GameObject mainCamera;

    //塗りつぶしてあるかどうか
    bool fillCheck;

   GameObject parent;

    private GameObject manager;

    //追加
    float pixelCounter;//色のあるピクセル数
    float pixelCheck = 0;
    bool fill;//塗りつぶし完了
    bool paintEnd;

    void Start()
    {
        Texture2D mainTexture = (Texture2D)gameObject. GetComponent<Renderer>().material.mainTexture;
        
        Color[] pixels = mainTexture.GetPixels();

        buffer = new Color[pixels.Length];
        pixels.CopyTo(buffer, 0);

        drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
        drawTexture.filterMode = FilterMode.Point;
        
        parent = gameObject.transform.parent.gameObject;
        paintEnd = false;

        manager = GameObject.Find("GameManager");

        //追加
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i].a >= 0.5f)
            {
                pixelCounter++;
            }
        }
        fill = false;

        mainCamera = GameObject.Find("Main Camera");

    }

    public void DrawLine(Vector2 p, Vector2 q)
    {
        //補間間隔
        var lerpNum = 15;
        for (int i = 0; i < lerpNum + 1; i++)
        {
            var r = Vector2.Lerp(p, q, i * (1.0f / lerpNum));
            Draw(r);
        }
    }

    public void Draw(Vector2 p)
    {
        p.x = (int)p.x;
        p.y = (int)p.y;


        var brushSize = 26f;
        
        var color = Color.black;

       
        for (int x = Mathf.Max(0, (int)(p.x - brushSize - 1)); x < Mathf.Min(drawTexture.width, (int)(p.x + brushSize + 1)); x++)
        {
            for (int y = Mathf.Max(0, (int)(p.y - brushSize - 1)); y < Mathf.Min(drawTexture.height, (int)(p.y + brushSize + 1)); y++)
            {
                if (Mathf.Pow(p.x - x, 2) + Mathf.Pow(p.y - y, 2) < Mathf.Pow(brushSize, 2))
                {
                    if (buffer[x + drawTexture.width * y].a >= 0.5
                        &&buffer[x + drawTexture.width * y].r!=0)
                    {
                        buffer.SetValue(color, x + drawTexture.width * y);
                        pixelCheck++;
                    }
                }
            }
        }
    }

    void Update()
    {
        //追加
        //黒く塗った割合
        float drawRate = pixelCheck / pixelCounter * 100f;

        //一定以上塗っていたら複製
        if (drawRate > 80)
        {
            fill = true;
        }

        GameProgress progress = manager.GetComponent<PlaySystem>().GetProgress();
        bool drawMode = manager.GetComponent<PlaySystem>().IsDrawMode();
        // サウンドを追加する時。追加したもの
        bool menuPauseFlag = manager.GetComponent<PlaySystem>().GetMenuPauseFlag();
        bool soundDebug = manager.GetComponent<PlaySystem>().GetSoundDebug();

        if (!menuPauseFlag && drawMode)
        {
            if (Input.GetMouseButtonDown(0) && !soundDebug)
            {
                SoundManager.Instance.Play_PaintStartSE();
                paintEnd = true;
            }
            if (Input.GetMouseButton(0) && !fillCheck)
            {

                //Ray ray = maker.GetComponent<MouseInput>().GetRay();
                //RaycastHit hit = maker.GetComponent<MouseInput>().GetHit();

                Ray ray = mainCamera.GetComponent<ImageProjection>().GetRay();
                RaycastHit hit = mainCamera.GetComponent<ImageProjection>().GetHit();

                if (Physics.Raycast(ray, out hit))
                {
                    //エラー対策（maker）がオブジェクトに触れてなければリターン
                    if (hit.collider == null || hit.collider.name == "Canvas" || hit.collider.gameObject.transform.parent.name != parent.transform.name)
                    {
                        //Start();
                        touching = false;
                        return;
                    }

                    var drawPoint = new Vector2(hit.textureCoord.x * drawTexture.width, hit.textureCoord.y * drawTexture.height);

                    //線を引いている最中なら
                    if (touching)
                    {
                        DrawLine(prevPoint, drawPoint);
                    }
                    //線の引き始め
                    else
                    {
                        //Debug.Log("      a");
                        Draw(drawPoint);

                    }
                    prevPoint = drawPoint;
                    touching = true;
                    objectName = hit.collider.name;

                    drawTexture.SetPixels(buffer);
                    drawTexture.Apply();

                    hit.collider.GetComponent<Renderer>().material.mainTexture = drawTexture;
                    parent.GetComponent<SpriteRenderer>().sprite = Sprite.Create(drawTexture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));

                }
                else
                {
                    touching = false;
                }

            }
            else
            {
                touching = false;
            }
        }

        if (Input.GetMouseButton(0) && !soundDebug && paintEnd)
        {
            SoundManager.Instance.Play_PaintEndSE();
            paintEnd = false;
        }

        //塗りつぶし処理
        if(parent.GetComponent<ColorChange>().GetAddChaeck()
            && !fillCheck)
        {
            FillBlack();
        }
    }

    //複製したオブジェクトに塗り残しがあれば黒く塗りつぶす。
    public void FillBlack()
    {
        if (parent.transform.name.Contains("Clone"))
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].a >= 0.5f&&buffer[i].r !=0)
                {
                    buffer.SetValue(Color.black, i);
                   
                }
            }
            fillCheck = true;
        }
        
        drawTexture.SetPixels(buffer);
        drawTexture.Apply();
        parent.GetComponent<SpriteRenderer>().sprite = Sprite.Create(drawTexture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }

    public bool GetFill()
    {
        return fill;
    }
}
