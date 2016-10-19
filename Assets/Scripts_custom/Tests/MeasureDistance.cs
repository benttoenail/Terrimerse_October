using UnityEngine;
using System.Collections;

public class MeasureDistance : MonoBehaviour {

	public GameObject measurementTool;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Measure();
	}

	void Measure() {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 100) && Input.GetMouseButtonDown(0)){
			
			print(hit.point);
			Vector3 spawnPos = new Vector3();
			Instantiate(measurementTool, spawnPos, Quaternion.identity);
				
		}

	}
}
