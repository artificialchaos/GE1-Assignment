using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject building;
    public GameObject roadseg1;
    public GameObject roadseg2;

    public Transform player;
    public int quadsPerTile;

    public int halfTile = 5;

    Vector3 startPos;

    class Tile
    {
        public GameObject theTile;
        public float creationTime;


        public Tile(GameObject t, float ct)
        {
            theTile = t;
            creationTime = ct;
        }
    }

    void Start()
    {
        //Vector3[] tileMeshVertices = tilePrefab.GetComponent<MeshCollider>().sharedMesh.vertices;
        //Debug.Log(tileMeshVertices);
        PerlinTerrain tt = tilePrefab.GetComponent<PerlinTerrain>();
        if (tt != null)
        {
            quadsPerTile = tt.quadsPerTile;
        }

        if (player == null)
        {
            player = Camera.main.transform;
        }

        StartCoroutine(GenerateWorldAroundPlayer());

    }

    Queue<GameObject> oldGameObjects = new Queue<GameObject>();
    Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    private IEnumerator GenerateWorldAroundPlayer()
    {
        int xMove = int.MaxValue;
        int zMove = int.MaxValue;

        while (true)
        {
            if (oldGameObjects.Count > 0)
            {
                GameObject.Destroy(oldGameObjects.Dequeue());
            }
            if (Mathf.Abs(xMove) >= quadsPerTile || Mathf.Abs(zMove) >= quadsPerTile)
            {
                float updateTime = Time.realtimeSinceStartup;

                int playerX = (int)(Mathf.Floor((player.transform.position.x) / (quadsPerTile)) * quadsPerTile);
                int playerZ = (int)(Mathf.Floor((player.transform.position.z) / (quadsPerTile)) * quadsPerTile);
                List<Vector3> newTiles = new List<Vector3>();
                for (int x = -halfTile; x < halfTile; x++)
                {
                    for (int z = -halfTile; z < halfTile; z++)
                    {
                        Vector3 pos = new Vector3((x * quadsPerTile + playerX),
                            0,
                            (z * quadsPerTile + playerZ));
                        string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                        if (!tiles.ContainsKey(tilename))
                        {
                            newTiles.Add(pos);
                        }
                        else
                        {
                            (tiles[tilename] as Tile).creationTime = updateTime;
                        }
                    }
                }
                newTiles.Sort((a, b) => (int)Vector3.SqrMagnitude(player.transform.position - a) - (int)Vector3.SqrMagnitude(player.transform.position - b));
                foreach (Vector3 pos in newTiles)
                {
                    //tile is created
                    //add code for building and road generation here?
                    GameObject t = GameObject.Instantiate<GameObject>(tilePrefab, pos, Quaternion.identity);
                    //GameObject b = GameObject.Instantiate<GameObject>(building, new Vector3(pos.x, pos.y + 15, pos.z), Quaternion.identity);
                    GameObject r1 = GameObject.Instantiate<GameObject>(roadseg1, new Vector3(pos.x, pos.y + 20, pos.z), Quaternion.identity);
                    GameObject r2 = GameObject.Instantiate<GameObject>(roadseg2, new Vector3(pos.x, pos.y + 20, pos.z), Quaternion.identity);
                    t.transform.parent = this.transform;
                    //b.transform.parent = this.transform;
                    string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                    t.name = tilename;
                    Tile tile = new Tile(t, updateTime);
                    tiles[tilename] = tile;
                    yield return null;
                }

                Dictionary<string, Tile> newTerrain = new Dictionary<string, Tile>();
                foreach (Tile tile in tiles.Values)
                {
                    if (tile.creationTime != updateTime)
                    {
                        oldGameObjects.Enqueue(tile.theTile);
                    }
                    else
                    {
                        newTerrain[tile.theTile.name] = tile;
                    }
                }
                tiles = newTerrain;
                startPos = player.transform.position;
            }
            yield return null;
            xMove = (int)(player.transform.position.x - startPos.x);
            zMove = (int)(player.transform.position.z - startPos.z);
        }
    }

    void Update()
    {

    }
}