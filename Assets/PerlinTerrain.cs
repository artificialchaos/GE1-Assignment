using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrain : MonoBehaviour //script to generate uneven terrain tile using perlin noise
{

    public int quadsPerTile = 10;
    public Material Material;
    public float amplitude = 50;
    Mesh m;

    private delegate float Cell(float x, float y);

    Cell[] cell = 
    {
               new Cell(Cell1)
              ,new Cell(Cell2)
    };

    public int cellselect = 0;

    Vector2 offset;
    void Awake()
    {
        
        offset = Random.insideUnitCircle * Random.Range(0, 1000);
        MeshFilter mf = gameObject.AddComponent<MeshFilter>(); 
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>(); 
        MeshCollider mc = gameObject.AddComponent<MeshCollider>();
        m = mf.mesh;

        int verticesPerQuad = 4;
        Vector3[] vertices = new Vector3[verticesPerQuad * quadsPerTile * quadsPerTile];
        Vector2[] uv = new Vector2[verticesPerQuad * quadsPerTile * quadsPerTile];

        int vertexTriangesPerQuad = 6;
        int[] triangles = new int[vertexTriangesPerQuad * quadsPerTile * quadsPerTile];

        Vector3 bottomLeft = new Vector3(-quadsPerTile / 2, 0, -quadsPerTile / 2);
        int vertex = 0;
        int triangleVertex = 0;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        for (int row = 0; row < quadsPerTile; row++)
        {
            //tile corner positions and vertices are calculated
            for (int col = 0; col < quadsPerTile; col++)
            {
                Vector3 bl = bottomLeft + new Vector3(col, cell[cellselect](transform.position.x + col, transform.position.z + row), row);
                Vector3 tl = bottomLeft + new Vector3(col, cell[cellselect](transform.position.x + col, transform.position.z + row + 1), row + 1);
                Vector3 tr = bottomLeft + new Vector3(col + 1, cell[cellselect](transform.position.x + col + 1, transform.position.z + row + 1), row + 1);
                Vector3 br = bottomLeft + new Vector3(col + 1, cell[cellselect](transform.position.x + col + 1, transform.position.z + row), row);

                int startVertex = vertex;
                vertices[vertex++] = bl;
                vertices[vertex++] = tl;
                vertices[vertex++] = tr;
                vertices[vertex++] = br;

                vertex = startVertex;
                float fNumQuads = quadsPerTile;
                uv[vertex++] = new Vector2(col / fNumQuads, row / fNumQuads);
                uv[vertex++] = new Vector2(col / fNumQuads, (row + 1) / fNumQuads);
                uv[vertex++] = new Vector2((col + 1) / fNumQuads, (row + 1) / fNumQuads);
                uv[vertex++] = new Vector2((col + 1) / fNumQuads, row / fNumQuads);

                triangles[triangleVertex++] = startVertex;
                triangles[triangleVertex++] = startVertex + 1;
                triangles[triangleVertex++] = startVertex + 3;
                triangles[triangleVertex++] = startVertex + 3;
                triangles[triangleVertex++] = startVertex + 1;
                triangles[triangleVertex++] = startVertex + 2;

                if (bl.y > maxY)
                {
                    maxY = bl.y;
                }
                if (bl.y < minY)
                {
                    minY = bl.y;
                }
            }
        }
        m.vertices = vertices;
        m.uv = uv;
        m.triangles = triangles;
        m.RecalculateNormals();
        mr.material = Material;
        mc.sharedMesh = m;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        mr.receiveShadows = true;
    }


    //procedural terrain generation methods using sin waves and perlin noise
    public static float Cell1(float x, float y)
    {
        return Mathf.Sin(Utility.Map(x, 0, 10, 0, Mathf.PI)) * Mathf.Sin(Utility.Map(y, 0, 10, 0, Mathf.PI)) * 40;
    }

    public static float Cell2(float x, float y)
    {
        return (Mathf.PerlinNoise(10000 + x / 100, 10000 + y / 100) * 2) + (Mathf.PerlinNoise(10000 + x / 1000, 10000 + y / 1000) * 5) 
        + (Mathf.PerlinNoise(1000 + x / 5, 100 + y / 5) * 1);
    }
}