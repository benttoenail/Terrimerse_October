using UnityEngine;
using System.Collections;

public class GetMeshVerts : MonoBehaviour {

	Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		print(mesh.vertices[0].x);
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}
}
