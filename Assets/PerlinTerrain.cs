using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    public float detail = 5.0f;
    public float height = 2.0f;

	// Use this for initialization
	void Start ()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for( int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = Mathf.PerlinNoise((vertices[i].x + this.transform.position.x)/detail, (vertices[i].z + this.transform.position.z)/detail) *height;
        }

        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
