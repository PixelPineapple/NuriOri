using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCleane_2 : MonoBehaviour
{

    Texture2D drawTexture;
    Color[] buffer;
    bool touching = false;
    Vector2 prevPoint;

    Sprite sprite;

    public GameObject mainCamera;

    private GameObject manager;

    //追加
    List<int> cleaner = new List<int>();
    List<Color> colore = new List<Color>();

    void Start()
    {
        Texture2D mainTexture = (Texture2D)gameObject.GetComponent<Renderer>().material.mainTexture;

        Color[] pixels = mainTexture.GetPixels();

        buffer = new Color[pixels.Length];

        pixels.CopyTo(buffer, 0);

        drawTexture = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);
        drawTexture.filterMode = FilterMode.Point;
        
        manager = GameObject.Find("GameManager");
    }

    public void DrawLine(Vector2 p, Vector2 q)
    {
        //補間間隔
        var lerpNum = 30;
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


        var brushSize = 3;

        for (int x = Mathf.Max(0, (int)(p.x - brushSize - 2)); x < Mathf.Min(drawTexture.width, (int)(p.x + brushSize + 2)); x++)
        {

            for (int y = Mathf.Max(0, (int)(p.y - brushSize - 2)); y < Mathf.Min(drawTexture.height, (int)(p.y + brushSize + 2)); y++)
            {
                float distance = Mathf.Abs(Vector2.Distance(p, new Vector2(x, y)));

                var color = new Color(0.05f, 0.05f, 0.05f, 1.0f);
                color = color * new Color(distance, distance, distance, 1.0f);

                Color color2 = new Color(0.01f * distance, 0.01f * distance, 0.01f * distance, 0.001f * distance);

                //円形に塗る
                if (Mathf.Pow(p.x - x, 2) + Mathf.Pow(p.y - y, 2) < Mathf.Pow(brushSize, 2.2f))
                {
                    if (buffer[x + drawTexture.width * y].a <= 0.5f
                        && buffer[x + drawTexture.width * y].r < 0.6f)
                    {

                        buffer.SetValue(color, x + drawTexture.width * y);

                        cleaner.Add(x + drawTexture.width * y);

                        colore.Add(color2);
                    }
                }

            }

        }

    }

    void Update()
    {
        GameProgress progress = manager.GetComponent<PlaySystem>().GetProgress();
        bool drawMode = manager.GetComponent<PlaySystem>().IsDrawMode();
        if (progress == GameProgress.Pause || !drawMode) return;
        CanvasInisialize2();

        //左クリックで色を塗る
        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.GetComponent<ImageProjection>().GetRay();
            RaycastHit hit = mainCamera.GetComponent<ImageProjection>().GetHit();

            if (Physics.Raycast(ray, out hit))
            {
                
                //エラー対策（maker）がオブジェクトに触れてなければリターン
                if (hit.collider == null || hit.collider.gameObject.name != transform.name)
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
                    Draw(drawPoint);
                }
                prevPoint = drawPoint;

                touching = true;

                drawTexture.SetPixels(buffer);
                drawTexture.Apply();

                hit.collider.GetComponent<Renderer>().material.mainTexture = drawTexture;

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

    //黒く塗られたところを徐々に白くする
    public void CanvasInisialize2()
    {
        List<int> inisialize = new List<int>();


        for (int i = 0; i < cleaner.Count; i++)
        {
            if (buffer[cleaner[i]].a >= 0)
            {
                buffer[cleaner[i]].a -= colore[i].a;

                colore[i] += new Color(0.00f, 0.00f, 0.00f, 0.005f);

            }
            else
            {
                inisialize.Add(i);
            }
        }

        drawTexture.SetPixels(buffer);
        drawTexture.Apply();

        GetComponent<Renderer>().material.mainTexture = drawTexture;

        for (int i = inisialize.Count - 1; i > 0; i--)
        {
            cleaner.RemoveAt(inisialize[i]);
            colore.RemoveAt(inisialize[i]);
        }

    }
}
