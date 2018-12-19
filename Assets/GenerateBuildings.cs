using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBuildings : MonoBehaviour
{
    public GameObject tallBuilding;
    public GameObject smallBuilding;


    void Start()
    {
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                System.Random rnd = new System.Random();
                int buildType = rnd.Next(1, 3);
                if (buildType == 1)
                {
                    Instantiate(tallBuilding, new Vector3(x * 10, 20, y * 10), Quaternion.identity);
                }
                else
                {
                    Instantiate(smallBuilding, new Vector3(x * 10, 15, y * 10), Quaternion.identity);
                }
                Debug.Log(buildType);
            }
        }
    }
    

}
