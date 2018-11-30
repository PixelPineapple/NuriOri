using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPaintBrush : MonoBehaviour
{

    Texture2D drawTexture;

    Color[] buffer;
    Color[] initializePixels;

    bool touching = false;
    Vector2 prevPoint;

    int initializeBrushSize;

    //GameObject maker;
    public GameObject mainCamera;

    //塗りつぶしてあるかどうか
    bool fillChaeck;

    //親オブジェクト
    GameObject parent;
    public ObjectGenerator objectGenerator;


    //初期Texture
    public Texture2D texture;

    private GameObject manager;
    private GameObject Generator;

    void Start()
    {
        Texture2D mainTexture = (Texture2D)gameObject.GetComponent<Renderer>().material.mainTexture;

        Color[] pixels = mainTexture.GetPixels();

        //初期textureのピクセル情報を格納
        initializePixels = texture.GetPixels();

        //bufferにmainTextureのピクセル情報をコピー
        buffer = new Color[pixels.Length];
        pixels.CopyTo(buffer, 0);

        drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
        drawTexture.filterMode = FilterMode.Point;


        parent = gameObject.transform.parent.gameObject;

        initializeBrushSize = 0;

        manager = GameObject.Find("GameManager");
        Generator = GameObject.Find("ObjectGenerator");
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

    #region 通常Draw
    public void Draw(Vector2 p)
    {
        p.x = (int)p.x;
        p.y = (int)p.y;


        var brushSize = 26f;

        var color = Color.black;

        //座標pを中心に円形に色を塗る
        for (int x = Mathf.Max(0, (int)(p.x - brushSize - 1)); x < Mathf.Min(drawTexture.width, (int)(p.x + brushSize + 1)); x++)
        {
            for (int y = Mathf.Max(0, (int)(p.y - brushSize - 1)); y < Mathf.Min(drawTexture.height, (int)(p.y + brushSize + 1)); y++)
            {
                if (Mathf.Pow(p.x - x, 2) + Mathf.Pow(p.y - y, 2) < Mathf.Pow(brushSize, 2))
                {
                    if (buffer[x + drawTexture.width * y].a >= 0.5
                        && buffer[x + drawTexture.width * y].r != 0)
                    {
                        buffer.SetValue(color, x + drawTexture.width * y);
                    }
                }
            }
        }
    }
    #endregion


    #region 色を元に戻すDraw
    public void Draw(Vector2 p, int brushSize)
    {
        p.x = (int)p.x;
        p.y = (int)p.y;

        brushSize = brushSize + initializeBrushSize;

        for (int x = Mathf.Max(0, (int)(p.x - brushSize - 1)); x < Mathf.Min(drawTexture.width, (int)(p.x + brushSize + 1)); x++)
        {
            for (int y = Mathf.Max(0, (int)(p.y - brushSize - 1)); y < Mathf.Min(drawTexture.height, (int)(p.y + brushSize + 1)); y++)
            {
                if (Mathf.Pow(p.x - x, 2) + Mathf.Pow(p.y - y, 2) < Mathf.Pow(brushSize, 2))
                {

                    buffer.SetValue(initializePixels[x + drawTexture.width * y], x + drawTexture.width * y);

                }
            }
        }

        initializeBrushSize++;

        if (brushSize >= 65)
        {
            fillChaeck = false;
            initializeBrushSize = 0;
            //複製可能な状態に戻す
            parent.GetComponent<ColorChange>().ChangeAddCheck(false);

        }

    }
    #endregion

    void Update()
    {
        GameProgress progress = manager.GetComponent<PlaySystem>().GetProgress();
        bool drawMode = manager.GetComponent<PlaySystem>().IsDrawMode();
        bool menuPauseFlag = manager.GetComponent<PlaySystem>().GetMenuPauseFlag();
        //if (progress == GameProgress.Pause || !drawMode) return;

        //if (isHeroStop || !isDrawMode) return;
        if (!menuPauseFlag && drawMode)
        {
            if (Input.GetMouseButton(0) && !fillChaeck)
            {
                Ray ray = mainCamera.GetComponent<ImageProjection>().GetRay();
                RaycastHit hit = mainCamera.GetComponent<ImageProjection>().GetHit();

                if (Physics.Raycast(ray, out hit))
                {
                    //エラー対策（maker）がオブジェクトに触れてなければリターン
                    if (hit.collider.name == "Canvas" || hit.transform.gameObject != gameObject)
                    {
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
                        Draw(drawPoint);

                    }
                    prevPoint = drawPoint;
                    touching = true;

                    //Drawで変更したbuffer情報をdrawTextureにセットしmainTeexture、spriteに反映
                    drawTexture.SetPixels(buffer);
                    drawTexture.Apply();

                    hit.transform.gameObject.GetComponent<Renderer>().material.mainTexture = drawTexture;
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

        //色を戻す
        if (fillChaeck)
        {
            Draw(new Vector2(drawTexture.width / 2, 64), 1);
            //Draw(new Vector2(drawTexture.width / 2, drawTexture.height / 2), 1);

            drawTexture.SetPixels(buffer);
            drawTexture.Apply();

            GetComponent<Renderer>().material.mainTexture = drawTexture;
            parent.GetComponent<SpriteRenderer>().sprite = Sprite.Create(drawTexture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
        }

        //塗りつぶし処理
        //if (parent.GetComponent<ColorChange>().GetAddChaeck() == true
        //    && !fillChaeck)
        if (objectGenerator.GetDupList().Count > 0)
        {
            FillBlack();
            //Debug.Log("        通過");
        }

    }

    //複製したオブジェクトに塗り残しがあれば黒く塗りつぶす。
    public void FillBlack()
    {

        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i].a >= 0.5f && buffer[i].r != 0)
            {
                buffer.SetValue(Color.black, i);

            }
        }
        fillChaeck = true;

        drawTexture.SetPixels(buffer);
        drawTexture.Apply();
        parent.GetComponent<SpriteRenderer>().sprite = Sprite.Create(drawTexture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
    }
}
