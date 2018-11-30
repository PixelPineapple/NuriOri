/*
 * 作成者 : シスワント　レサ 
 * 作成日 : 2018.04.13
 * 内容   : 紙を折る
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    LEFT,
    RIGHT,
    BOTTOM,
    UP,
    NONE
}

[RequireComponent(typeof(ProceduralGrid))]
public class FoldingMech : MonoBehaviour {

    #region Procedural Mesh Component
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] originalVert;
    #endregion

    #region Paper Component
    private Side dir;
    private Vector3 mousePosOrigin;
    private int size;
    private bool isClicked;
    private bool drawHighlight;
    private bool isFolding;
    private float paperHeight;
    private float paperWidth;
    float degree = 0;
    private float centerPoint = -1;
    #endregion

    #region Required Components
    private LineRenderer lineRenderer;
    private List<GameObject> lineList;
    public GameObject lineHighlightPrefabs;
    public PlaySystem playSystem;
    public Transform foldingAnchorPoint;
    public ObjectGenerator objectGenerator;
    //public ColorChange colorChange;
    #endregion


    // Use this for initialization
    void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineList = new List<GameObject>();
        dir = Side.NONE;
        mousePosOrigin = new Vector3(-1, -1, 0);
        ResetHighlight();
        isClicked = false;
        drawHighlight = false;
        isFolding = false;
        GetObjectComponent();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(centerPoint);

        // colorChange.GenerateInkedObject();
    }

    // Which side of the paper is pressed and highlight it
    public void FoldingHighlight(Vector3 point)
    {
        //Debug.Log("is Folding " + isFolding);
        //Debug.Log("is Clicked " + isClicked);
        if (isClicked || isFolding) return;

        isClicked = true;

        if (point.y >= 11.52f)
        {
            lineRenderer.SetPosition(0, new Vector3(0, paperHeight, 0));
            lineRenderer.SetPosition(1, new Vector3(paperWidth, paperHeight, 0));
            dir = Side.UP;
        }
        else if (point.y <= 1.28f)
        {
            lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
            lineRenderer.SetPosition(1, new Vector3(paperWidth, 0, 0));
            dir = Side.BOTTOM;
        }
        else
        {
            if (point.x <= 1.28f) // 2 / 16 * paperWidth
            {
                lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
                lineRenderer.SetPosition(1, new Vector3(0, paperHeight, 0));
                dir = Side.LEFT;
            }
            else if (point.x > 8.96f) // 14 / 16 * paperWidth
            {
                //Debug.Log((float)14 / 16 * paperWidth);
                lineRenderer.SetPosition(0, new Vector3(paperWidth, 0, 0));
                lineRenderer.SetPosition(1, new Vector3(paperWidth, paperHeight, 0));
                dir = Side.RIGHT;
            }
            else
            {
                dir = Side.NONE;
                return;
            }
        }
        if (!drawHighlight)
        {
            drawHighlight = true;
            GetFoldableSide();
        }
    }

    public void GetFoldableSide()
    {
        if (dir == Side.NONE || isFolding) return;

        //Vector3[] foldableVert = new Vector3[size * size - 1];
        Vector3[] foldableVert = new Vector3[2 * size - 2];
        float xPoint = GetComponent<ProceduralGrid>().GetWidthPerTile();
        float yPoint = GetComponent<ProceduralGrid>().GetHeightPerTile();
        int ind = 0;

        switch(dir)
        {
            case Side.LEFT:
            case Side.RIGHT:
                for (float x = xPoint; x < paperWidth; x+= xPoint)
                {
                    for (float y = 0; y <= paperHeight; y += paperHeight, ind++)
                    {
                        foldableVert[ind] = new Vector3(x, y);
                    }
                }
                break;

            case Side.UP:
            case Side.BOTTOM:
                for (float y = yPoint; y < paperHeight; y += yPoint)
                {
                    for (float x = 0; x <= paperWidth; x += paperWidth, ind++)
                    {
                        foldableVert[ind] = new Vector3(x, y);
                    }
                }
                break;
                
        }
        
        FoldMark(foldableVert);
    }

    // 折り可能線をハイライト
    public void FoldMark(Vector3[] foldableVert)
    {
        int index = 0;
        for (float i = 0; i < (size - 1); i++, index+=2)
        {
            GameObject line = Instantiate(lineHighlightPrefabs, new Vector3(
                foldableVert[index].x,
                foldableVert[index].y,
                foldableVert[index].z), Quaternion.identity);
            line.GetComponent<LineRenderer>().SetPosition(0, foldableVert[index]);
            line.GetComponent<LineRenderer>().SetPosition(1, foldableVert[index+1]);
            lineList.Add(line);
        }
    }


    public void FirstInput(Vector3 point)
    {
        if (mousePosOrigin.x < 0 || isFolding)
        {
            mousePosOrigin = point;
            return;
        }

        centerPoint = ComparePosition(point);
    }

    public float ComparePosition(Vector3 point)
    {
        float center = -1.0f;
        //Vector3 centerPos = Vector3.zero;
        switch(dir)
        {
            case Side.LEFT:
                foreach(GameObject line in lineList)
                {
                    if (line.GetComponent<LineRenderer>().GetPosition(0).x < mousePosOrigin.x) continue;

                    if (line.GetComponent<LineRenderer>().GetPosition(0).x < point.x)
                    {
                        //foldCenter = line;
                        center = line.GetComponent<LineRenderer>().GetPosition(0).x;
                        line.GetComponent<LineRenderer>().startColor = Color.red;
                        line.GetComponent<LineRenderer>().endColor = Color.red;
                    }
                    else
                    {
                        line.GetComponent<LineRenderer>().startColor = Color.yellow;
                        line.GetComponent<LineRenderer>().endColor = Color.yellow;
                    }
                }
                break;

            case Side.RIGHT:
                foreach (GameObject line in lineList)
                {
                    if (line.GetComponent<LineRenderer>().GetPosition(0).x > mousePosOrigin.x) continue;

                    if (line.GetComponent<LineRenderer>().GetPosition(0).x > point.x)
                    {
                        center = line.GetComponent<LineRenderer>().GetPosition(0).x;
                        line.GetComponent<LineRenderer>().startColor = Color.red;
                        line.GetComponent<LineRenderer>().endColor = Color.red;
                        break;
                    }
                    else
                    {
                        line.GetComponent<LineRenderer>().startColor = Color.yellow;
                        line.GetComponent<LineRenderer>().endColor = Color.yellow;
                    }
                }
                break;

            case Side.BOTTOM:
                foreach (GameObject line in lineList)
                {
                    if (line.GetComponent<LineRenderer>().GetPosition(0).y < mousePosOrigin.y) continue;

                    if (line.GetComponent<LineRenderer>().GetPosition(0).y < point.y)
                    {
                        //foldCenter = line;
                        center = line.GetComponent<LineRenderer>().GetPosition(0).y;
                        line.GetComponent<LineRenderer>().startColor = Color.red;
                        line.GetComponent<LineRenderer>().endColor = Color.red;
                    }
                    else
                    {
                        line.GetComponent<LineRenderer>().startColor = Color.yellow;
                        line.GetComponent<LineRenderer>().endColor = Color.yellow;
                    }

                }
                break;

            case Side.UP:
                foreach (GameObject line in lineList)
                {
                    if (line.GetComponent<LineRenderer>().GetPosition(0).y > mousePosOrigin.y) continue;

                    if (line.GetComponent<LineRenderer>().GetPosition(0).y > point.y)
                    {
                        center = line.GetComponent<LineRenderer>().GetPosition(0).y;
                        line.GetComponent<LineRenderer>().startColor = Color.red;
                        line.GetComponent<LineRenderer>().endColor = Color.red;
                        break;
                    }
                    else
                    {
                        line.GetComponent<LineRenderer>().startColor = Color.yellow;
                        line.GetComponent<LineRenderer>().endColor = Color.yellow;
                    }
                }
                break;

            default:
                center = -1;
                break;
        }

        //foldingAnchorPoint = Instantiate(foldingAnchorPoint, centerPos, Quaternion.identity) as Transform;
        return center;
    }

    public Vector3 MakeAnchorPoint(float center)
    {
        switch (dir)
        {
            case Side.LEFT:
            case Side.RIGHT:
                return new Vector3(center, 0, 0);
            case Side.UP:
            case Side.BOTTOM:
                return new Vector3(0, center, 0);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 紙を折る。
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    IEnumerator Folding(float center)
    {
        isFolding = true;

        // 音が鳴らせる
        bool soundDebug = playSystem.GetComponent<PlaySystem>().GetSoundDebug();
        if (!soundDebug)
        {
            SoundManager.Instance.Play_FoldSE();
        }
        //playSystem.Pause();
        switch (dir)
        {
            case Side.LEFT:
                while (degree >= -180)
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (originalVert[id].x >= center) continue;

                        vertices[id].x = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Sin(degree * Mathf.Deg2Rad) + 0;
                        
                        mesh.vertices = vertices;
                    }
                    degree -= 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(.1f);
                }
                break;

            case Side.RIGHT:
                while (degree <= 180)
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (originalVert[id].x <= center) continue;

                        vertices[id].x = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Sin(degree * Mathf.Deg2Rad) + 0;

                        mesh.vertices = vertices;
                    }
                    degree += 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                break;

            case Side.BOTTOM:
                while (degree <= 180)
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (originalVert[id].y >= center) continue;

                        vertices[id].y = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Sin(degree * Mathf.Deg2Rad) + 0;

                        mesh.vertices = vertices;
                    }
                    degree += 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                break;

            case Side.UP:
                while (degree >= -180 )
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (originalVert[id].y <= center) continue;

                        vertices[id].y = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Sin(degree * Mathf.Deg2Rad) + 0;

                        mesh.vertices = vertices;
                    }
                    degree -= 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                break;
        }

        yield return new WaitForSeconds(1f);

        if (degree < -180) degree = -180;
        else if (degree > 180) degree = 180;
        // コピーしたい物のコードはここに！
        StartCoroutine(Unfolding(center));
    }

    /// <summary>
    /// 折った状態紙を開く「基に戻る」
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    IEnumerator Unfolding(float center)
    {
        //Debug.Log("CenterPoint " + centerPoint);
        //Debug.Log("Paper Width " + paperWidth);
        //Debug.Log("Paper Height " + paperHeight);
        //Debug.Log("Dir" + dir);
        //Debug.Log("Degree = " + degree);
        objectGenerator.GenerateDuplicate(centerPoint, dir, paperHeight, paperWidth);

        // 音が鳴らせる
        bool soundDebug = playSystem.GetComponent<PlaySystem>().GetSoundDebug();
        if (!soundDebug)
        {
            SoundManager.Instance.Play_FoldSE();
        }
        
        switch (dir)
        {
            case Side.LEFT:
                while (degree <= 0)
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (vertices[id].z == 0) continue;
                       
                        vertices[id].x = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Sin(degree * Mathf.Deg2Rad) + 0;
                        mesh.vertices = vertices;

                    }
                    degree += 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(.1f);
                }
                break;
            case Side.RIGHT:
                while (degree >= 0)
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (vertices[id].z == 0) continue;

                        vertices[id].x = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].x - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Sin(degree * Mathf.Deg2Rad) + 0;

                        mesh.vertices = vertices;
                    }
                    degree -= 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                break;
            case Side.BOTTOM:
                while (degree >= 0)
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (vertices[id].z == 0) continue;

                        vertices[id].y = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * -Mathf.Sin(degree * Mathf.Deg2Rad) + 0;

                        mesh.vertices = vertices;
                    }
                    degree -= 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                break;
            case Side.UP:
                while (degree <= 0)
                {
                    for (int id = 0; id < vertices.Length; id++)
                    {
                        if (vertices[id].z == 0) continue;
                        
                        vertices[id].y = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Cos(degree * Mathf.Deg2Rad) + center;
                        vertices[id].z = Mathf.Sqrt(Mathf.Pow((originalVert[id].y - center), 2) + (Mathf.Pow(originalVert[id].z, 2))) * Mathf.Sin(degree * Mathf.Deg2Rad) + 0;

                        mesh.vertices = vertices;
                    }
                    degree += 36;

                    // ゲームがポーズされたら
                    while (playSystem.GetMenuPauseFlag())
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(.1f);
                }
                break;
        }

        // 折りサイドを最初の状態に戻す。
        isFolding = false;
        dir = Side.NONE;
        centerPoint = -1;
        degree = 0;

        if (objectGenerator.GetDupList().Count > 0)
        {
            foreach (GameObject x in objectGenerator.GetDupList())
            {
                x.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }
        objectGenerator.ClearDuplicatedObjList();
    }


    // 紙がタッチされていない。
    public void ResetHighlight()
    {
        if(centerPoint > 0 && !isFolding)
        {
            StartCoroutine(Folding(centerPoint));
        }
        
        for (int lrp = 0; lrp < lineRenderer.positionCount; lrp++)
        {
            lineRenderer.SetPosition(lrp, Vector3.zero);
        }
        mousePosOrigin.x = -1;
        isClicked = false;
        drawHighlight = false;
        //centerPoint = -1;
        // リストをクリア
        foreach(GameObject line in lineList)
        {
            Destroy(line);
        }
        lineList.Clear();
    }

    private void GetObjectComponent()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        originalVert = mesh.vertices;
        size = GetComponent<ProceduralGrid>().size;
        paperHeight = GetComponent<ProceduralGrid>().GetHeightPerTile() * size;
        paperWidth = GetComponent<ProceduralGrid>().GetWidthPerTile() * size;
    }

    public bool GetIsFolding()
    {
        return isFolding;
    }

    public float GetCenterPoint()
    {
        return centerPoint;
    }

    public Side GetSide()
    {
        return dir;
    }
}
