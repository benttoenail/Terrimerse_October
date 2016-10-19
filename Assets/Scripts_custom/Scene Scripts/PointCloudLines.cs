using UnityEngine;
using System.Collections;

public class PointCloudLines : MonoBehaviour {

	Vector3[] points;
	int count;
	Mesh mesh;

	//GameObject quad;

	//LineRenderer line;

	// Use this for initialization
	void Start () {
		
		mesh = gameObject.GetComponent<MeshFilter>().mesh;
	//	line = gameObject.GetComponent<LineRenderer>();

		count = mesh.vertices.Length;

		points = new Vector3[count];

		//quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

		for(int i = 0; i < count; i++){
			points[i] = mesh.vertices[i];

		}

		for(int i = 0; i < count; i++){
			GameObject quadPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);
			quadPlane.transform.position = points[i];
		}

	}
	
	// Update is called once per frame
	void Update () {



	}
}
