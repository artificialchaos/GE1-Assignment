using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour {

	void Start () {
		
	}
	
	//Moves car forward
	void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * 5);
    }
}
