using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    //define prefabs
    public GameObject tilePrefab;
    public GameObject tallbuilding;
    public GameObject house;
    public GameObject car;
    public GameObject roadseg1;
    public GameObject roadseg2;
    public Transform player;

    public int quadsPerTile;
    public int halfTile = 5;
    Vector3 startPos;

    //classes for each type of object that will be generated 
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

    class Building
    {
        public GameObject theBuilding;
        public float creationTime;


        public Building(GameObject bd, float ct)
        {
            theBuilding = bd;
            creationTime = ct;
        }
    }

    void Start()
    {
        PerlinTerrain pt = tilePrefab.GetComponent<PerlinTerrain>();
        if (pt != null)
        {
            quadsPerTile = pt.quadsPerTile;
        }

        if (player == null)
        {
            player = Camera.main.transform;
        }

        StartCoroutine(GenerateWorld());

    }

    //objects to be removed from the scene
    Queue<GameObject> oldGameObjects = new Queue<GameObject>();
    //dictionaries for keeping track of each object 
    Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    Dictionary<string, Road> roads = new Dictionary<string, Road>();
    Dictionary<string, Building> buildings = new Dictionary<string, Building>();

    //array containing values representing the 4 directions the car can face 
    int[] carRotation = { 0, 90, 180, 270 };

    private IEnumerator GenerateWorld()
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

                //lists of objects to be generated
                List<Vector3> newTiles = new List<Vector3>();
                List<Vector3> newRoads = new List<Vector3>();
                List<Vector3> newBuildings = new List<Vector3>();


                for (int x = -halfTile; x < halfTile; x++)
                {
                    for (int z = -halfTile; z < halfTile; z++)
                    {
                        Vector3 pos = new Vector3((x * quadsPerTile + playerX), 0, (z * quadsPerTile + playerZ));

                        //
                        string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                        string roadname1 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                        string roadname2 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";

                        string towername1 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                        string towername2 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";
                        string towername3 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_3";
                        string towername4 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_4";

                        string housename1 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                        string housename2 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";
                        string housename3 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_3";
                        string housename4 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_4";
                        string housename5 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_5";
                        string housename6 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_6";
                        string housename7 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_7";
                        string housename8 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_8";

                        //objects added to hash tables
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

                        if (!buildings.ContainsKey(towername1))
                        {
                            newBuildings.Add(pos);
                        }
                        else
                        {
                            (buildings[towername1] as Building).creationTime = updateTime;
                            (buildings[towername2] as Building).creationTime = updateTime;
                            (buildings[towername3] as Building).creationTime = updateTime;
                            (buildings[towername4] as Building).creationTime = updateTime;
                            (buildings[housename1] as Building).creationTime = updateTime;
                            (buildings[housename2] as Building).creationTime = updateTime;
                            (buildings[housename3] as Building).creationTime = updateTime;
                            (buildings[housename4] as Building).creationTime = updateTime;
                            (buildings[housename5] as Building).creationTime = updateTime;
                            (buildings[housename6] as Building).creationTime = updateTime;
                            (buildings[housename7] as Building).creationTime = updateTime;
                            (buildings[housename8] as Building).creationTime = updateTime;
                        }
                    }
                }
                //sort objects by distance from player
                newTiles.Sort((a, b) => (int)Vector3.SqrMagnitude(player.transform.position - a) - (int)Vector3.SqrMagnitude(player.transform.position - b));
                newRoads.Sort((a, b) => (int)Vector3.SqrMagnitude(player.transform.position - a) - (int)Vector3.SqrMagnitude(player.transform.position - b));
                newBuildings.Sort((a, b) => (int)Vector3.SqrMagnitude(player.transform.position - a) - (int)Vector3.SqrMagnitude(player.transform.position - b));

                foreach (Vector3 pos in newTiles)
                {
                    System.Random rnd = new System.Random();

                    //tile is created
                    GameObject t = GameObject.Instantiate<GameObject>(tilePrefab, pos, Quaternion.identity);
                    t.transform.parent = this.transform;

                    //car direction is chosen randomly
                    int direction = rnd.Next(0, 4);
                    int rotation = carRotation[direction];
                    GameObject cr = GameObject.Instantiate<GameObject>(car, new Vector3(pos.x, pos.y + 6, pos.z), Quaternion.Euler(0, rotation, 0));

                    GameObject r1 = GameObject.Instantiate<GameObject>(roadseg1, new Vector3(pos.x, pos.y + 4, pos.z), Quaternion.identity);
                    GameObject r2 = GameObject.Instantiate<GameObject>(roadseg2, new Vector3(pos.x, pos.y + 4, pos.z), Quaternion.identity);

                    //random variables dictating position objects are created relative to tile
                    int xdisplacement = rnd.Next(10, 35);
                    int ydisplacement = rnd.Next(10, 35);

                    int house_displacement_1 = rnd.Next(4, 8);
                    int house_displacement_2 = rnd.Next(4, 8);

                    GameObject h1 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + house_displacement_1, pos.y + 8, pos.z + ydisplacement), Quaternion.identity);
                    GameObject h2 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + xdisplacement, pos.y + 8, pos.z + house_displacement_2), Quaternion.identity);

                    GameObject h3 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - house_displacement_1, pos.y + 8, pos.z + ydisplacement), Quaternion.identity);
                    GameObject h4 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - xdisplacement, pos.y + 8, pos.z + house_displacement_2), Quaternion.identity);

                    GameObject h5 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + house_displacement_1, pos.y + 8, pos.z - ydisplacement), Quaternion.identity);
                    GameObject h6 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x + xdisplacement, pos.y + 8, pos.z - house_displacement_2), Quaternion.identity);

                    GameObject h7 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - house_displacement_1, pos.y - 8, pos.z - ydisplacement), Quaternion.identity);
                    GameObject h8 = GameObject.Instantiate<GameObject>(house, new Vector3(pos.x - xdisplacement, pos.y + 8, pos.z - house_displacement_2), Quaternion.identity);


                    //height of towers is randomized
                    int b1_height = rnd.Next(20, 60);
                    int b2_height = rnd.Next(20, 60);
                    int b3_height = rnd.Next(20, 60);
                    int b4_height = rnd.Next(20, 60);

                  
                    GameObject b1 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x + xdisplacement, pos.y + 10, pos.z + ydisplacement), Quaternion.identity);
                    b1.transform.localScale = new Vector3(5, b1_height, 5);
                    GameObject b2 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x - xdisplacement, pos.y + 10, pos.z + ydisplacement), Quaternion.identity);
                    b2.transform.localScale = new Vector3(5, b2_height, 5);
                    GameObject b3 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x + xdisplacement, pos.y + 10, pos.z - ydisplacement), Quaternion.identity);
                    b3.transform.localScale = new Vector3(5, b3_height, 5);
                    GameObject b4 = GameObject.Instantiate<GameObject>(tallbuilding, new Vector3(pos.x - xdisplacement, pos.y + 10, pos.z - ydisplacement), Quaternion.identity);
                    b4.transform.localScale = new Vector3(5, b4_height, 5);

                    //each object must have a name for reference
                    string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                    string roadname1 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                    string roadname2 = "Road_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";

                    string towername1 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                    string towername2 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";
                    string towername3 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_3";
                    string towername4 = "Tower_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_4";

                    string housename1 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_1";
                    string housename2 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_2";
                    string housename3 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_3";
                    string housename4 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_4";
                    string housename5 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_5";
                    string housename6 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_6";
                    string housename7 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_7";
                    string housename8 = "House_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString() + "_8";


                    t.name = tilename;
                    r1.name = roadname1;
                    r2.name = roadname2;

                    h1.name = housename1;
                    h2.name = housename2;
                    h3.name = housename3;
                    h4.name = housename4;
                    h5.name = housename5;
                    h6.name = housename6;
                    h7.name = housename7;
                    h8.name = housename8;

                    b1.name = towername1;
                    b2.name = towername2;
                    b3.name = towername3;
                    b4.name = towername4;

                    //objects are instantiated and stored
                    Road road1 = new Road(r1, updateTime);
                    Road road2 = new Road(r2, updateTime);
                    Tile tile = new Tile(t, updateTime);

                    Building tower1 = new Building(b1, updateTime);
                    Building tower2 = new Building(b2, updateTime);
                    Building tower3 = new Building(b3, updateTime);
                    Building tower4 = new Building(b4, updateTime);

                    Building house1 = new Building(h1, updateTime);
                    Building house2 = new Building(h2, updateTime);
                    Building house3 = new Building(h3, updateTime);
                    Building house4 = new Building(h4, updateTime);
                    Building house5 = new Building(h5, updateTime);
                    Building house6 = new Building(h6, updateTime);
                    Building house7 = new Building(h7, updateTime);
                    Building house8 = new Building(h8, updateTime);


                    tiles[tilename] = tile;
                    roads[roadname1] = road1;
                    roads[roadname2] = road2;

                    buildings[towername1] = tower1;
                    buildings[towername2] = tower2;
                    buildings[towername3] = tower3;
                    buildings[towername4] = tower4;
                    buildings[housename1] = house1;
                    buildings[housename2] = house2;
                    buildings[housename3] = house3;
                    buildings[housename4] = house4;
                    buildings[housename5] = house5;
                    buildings[housename6] = house6;
                    buildings[housename7] = house7;
                    buildings[housename8] = house8;

                    yield return null;
                }

                //newly created tiles stored in hashtables with tiles still close to the player
                Dictionary<string, Tile> newTerrain = new Dictionary<string, Tile>();
                Dictionary<string, Road> newRoadSystem = new Dictionary<string, Road>();
                Dictionary<string, Building> newSkyline = new Dictionary<string, Building>();

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
                foreach (Building building in buildings.Values)
                {
                    if (building.creationTime != updateTime)
                    {
                        oldGameObjects.Enqueue(building.theBuilding);
                    }
                    else
                    {
                        newSkyline[building.theBuilding.name] = building;
                    }
                }
                roads = newRoadSystem;
                tiles = newTerrain;
                buildings = newSkyline;
                startPos = player.transform.position;
            }
            yield return null;
            //calculate change in player position
            xMove = (int)(player.transform.position.x - startPos.x);
            zMove = (int)(player.transform.position.z - startPos.z);
        }
    }

    void Update()
    {

    }
}