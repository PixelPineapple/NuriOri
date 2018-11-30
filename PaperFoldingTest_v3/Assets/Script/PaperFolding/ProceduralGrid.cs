using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour {
    
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private float gameObjectSize;
    public float heightSquareCount;
    public float widthSquareCount;
    private float heightPerTile;
    private float widthPerTile;


    public int size = 0;

    private void Awake()
    {
        gameObjectSize = 0.64f;

        widthPerTile = gameObjectSize * widthSquareCount;
        heightPerTile = gameObjectSize * heightSquareCount;

        float colliderWidth = widthPerTile * size;
        float colliderHeight = heightPerTile * size;

        // Setting Box Collider Size and Center
        //gameObject.GetComponent<BoxCollider>().size = new Vector3(
        //    colliderWidth,
        //    colliderHeight,
        //    0.0f);

        //gameObject.GetComponent<BoxCollider>().center = new Vector3(
        //    colliderWidth / 2,
        //    colliderHeight / 2,
        //    0.0f);

        // 1 tile → 4 square ↑ 5 square
        Generate();
    }

    // Use this for initialization
    void Start ()
    {

	}

    // 紙を作る。
    void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        // Generate Vertex
        vertices = new Vector3[(size + 1) * (size + 1) * 2];
        Vector2[] uv = new Vector2[vertices.Length];
        int flipSide = 0, i = 0;
        while (flipSide < 2)
        {
            for (float y = 0; y <= heightPerTile * size; y += heightPerTile)
            {
                for (float x = 0; x <= widthPerTile * size; x += widthPerTile, i++)
                {
                    vertices[i] = new Vector3(x, y);
                    uv[i] = new Vector2(x / (widthPerTile * size), y / (heightPerTile * size));
                }
            }
            
            flipSide++;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;

        // Generate Triangles
        triangles = new int[size * size * 6 * 2];
        //triangles = new int[size * size * 6];

        int vi = 0, ti = 0;
        // frontside triangle
        for (int y = 0; y < size; y++, vi++)
        {
            for (int x = 0; x < size; ti += 6, vi++, x++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + size + 1;
                triangles[ti + 5] = vi + size + 2;
            }
        }

        vi = vi + (size + 1);
        
        // backside triangle
        for (int y = 0; y < size; y++, vi++)
        {
            for (int x = 0; x < size; ti += 6, vi++, x++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + size + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + 1;
                triangles[ti + 5] = vi + size + 2;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    // 紙のVerticesを見えるように。
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    // Getter メソッド
    public Mesh GetMesh() { return mesh; }

    public Vector3[] GetVertices() { return vertices; }

    public int[] GetTris() { return triangles; }

    public float GetGameObjSize()
    {
        return gameObjectSize;
    }

    public float GetWidthPerTile()
    {
        return widthPerTile;
    }

    public float GetHeightPerTile()
    {
        return heightPerTile;
    }
}
