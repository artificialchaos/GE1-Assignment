using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject tallbuilding;
    public GameObject house;
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


        public Tile(GameObject tm, float ct)
        {
            theTile = tm;
            creationTime = ct;
        }
    }

    class Road
    {
        public GameObject theRoad;
        public float creationTime;


        public Road(GameObject rd, float ct)
        {
            theRoad = rd;
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
    Dictionary<string, Road> roads = new Dictionary<string, Road>();

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
                List<Vector3> newRoads = new List<Vector3>();


                for (int x = -halfTile; x < halfTile; x++)
                {
                    for (int z = -halfTile; z < halfTile; z++)
                    {
                        Vector3 pos = new Vector3((x * quadsPerTile + playerX), 0, (z * quadsPerTile + playerZ));

                        string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                        string roadname1 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                        string roadname2 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";

                        if (!tiles.ContainsKey(tilename))
                        {
                            newTiles.Add(pos);
                        }
                        else
                        {
                            (tiles[tilename] as Tile).creationTime = updateTime;
                        }

                        if (!roads.ContainsKey(roadname1))
                        {
                            newRoads.Add(pos);
                        }
                        else
                        {
                            (roads[roadname1] as Road).creationTime = updateTime;
                            (roads[roadname2] as Road).creationTime = updateTime;
                        }
                    }
                }

                newTiles.Sort((a, b) => (int)Vector3.SqrMagnitude(player.transform.position - a) - (int)Vector3.SqrMagnitude(player.transform.position - b));
                newRoads.Sort((a, b) => (int)Vector3.SqrMagnitude(player.transform.position - a) - (int)Vector3.SqrMagnitude(player.transform.position - b));

                foreach (Vector3 pos in newTiles)
                {
                    System.Random rnd = new System.Random();

                    GameObject t = GameObject.Instantiate<GameObject>(tilePrefab, pos, Quaternion.identity);
                    t.transform.parent = this.transform;

                    GameObject r1 = GameObject.Instantiate<GameObject>(roadseg1, new Vector3(pos.x, pos.y + 4, pos.z), Quaternion.identity);
                    GameObject r2 = GameObject.Instantiate<GameObject>(roadseg2, new Vector3(pos.x, pos.y + 4, pos.z), Quaternion.identity);

                    //GameObject b1 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(rnd.Next((int)pos.x + 6, (int)pos.x + 70), 
                    //   pos.y + 4, rnd.Next((int)pos.x + 6, (int)pos.x + 70)), Quaternion.identity);
                    int xdisplacement = rnd.Next(10, 35);
                    int ydisplacement = rnd.Next(10, 35);

                    int house_displacement_1 = rnd.Next(3, 8);
                    int house_displacement_2 = rnd.Next(3, 8);

                    GameObject h1 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + house_displacement_1, pos.y + 8, pos.z + ydisplacement), Quaternion.identity);
                    GameObject h2 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + xdisplacement, pos.y + 8, pos.z + house_displacement_2), Quaternion.identity);

                    GameObject h3 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - house_displacement_1, pos.y + 8, pos.z + ydisplacement), Quaternion.identity);
                    GameObject h4 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - xdisplacement, pos.y + 8, pos.z + house_displacement_2), Quaternion.identity);

                    GameObject h5 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + house_displacement_1, pos.y + 8, pos.z - ydisplacement), Quaternion.identity);
                    GameObject h6 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + xdisplacement, pos.y + 8, pos.z - house_displacement_2), Quaternion.identity);

                    GameObject h7 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - house_displacement_1, pos.y - 8, pos.z + ydisplacement), Quaternion.identity);
                    GameObject h8 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - xdisplacement, pos.y + 8, pos.z - house_displacement_2), Quaternion.identity);



                    int b1_height = rnd.Next(20, 50);
                    int b2_height = rnd.Next(20, 50);
                    int b3_height = rnd.Next(20, 50);
                    int b4_height = rnd.Next(20, 50);

                    GameObject b1 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x + xdisplacement, pos.y + 10, pos.z + ydisplacement), Quaternion.identity);
                    b1.transform.localScale = new Vector3(5, b1_height, 5);
                    GameObject b2 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x - xdisplacement, pos.y + 10, pos.z + ydisplacement), Quaternion.identity);
                    b2.transform.localScale = new Vector3(5, b2_height, 5);
                    GameObject b3 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x + xdisplacement, pos.y + 10, pos.z - ydisplacement), Quaternion.identity);
                    b3.transform.localScale = new Vector3(5, b3_height, 5);
                    GameObject b4 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x - xdisplacement, pos.y + 10, pos.z - ydisplacement), Quaternion.identity);
                    b4.transform.localScale = new Vector3(5, b4_height, 5);

                    string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                    string roadname1 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                    string roadname2 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";

                    t.name = tilename;
                    r1.name = roadname1;
                    r2.name = roadname2;

                    Road road1 = new Road(r1, updateTime);
                    Road road2 = new Road(r2, updateTime);
                    Tile tile = new Tile(t, updateTime);

                    tiles[tilename] = tile;
                    roads[roadname1] = road1;
                    roads[roadname2] = road2;

                    yield return null;
                }

                Dictionary<string, Tile> newTerrain = new Dictionary<string, Tile>();
                Dictionary<string, Road> newRoadSystem = new Dictionary<string, Road>();

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
                foreach (Road road in roads.Values)
                {
                    if (road.creationTime != updateTime)
                    {
                        oldGameObjects.Enqueue(road.theRoad);
                    }
                    else
                    {
                        newRoadSystem[road.theRoad.name] = road;
                    }
                }
                roads = newRoadSystem;
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