using UnityEngine;
using System.Collections;

public class ListPointClouds : MonoBehaviour {

    Transform[] pointCloud;
    int childCount;

    Mesh mesh;
    LineRenderer line;

	// Use this for initialization
	void Start () {
        
        childCount = transform.childCount;

        pointCloud = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            pointCloud[i] = transform.GetChild(i);
            pointCloud[i].gameObject.AddComponent<LineRenderer>();
        }
            
    }
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < childCount; i++)
        {
            mesh = pointCloud[i].GetComponent<MeshFilter>().mesh;

            Vector3 point1 = new Vector3(mesh.vertices[0].x, mesh.vertices[0].y, mesh.vertices[0].z);
            Vector3 point2 = new Vector3(point1.x, point1.y, -100);

            line = pointCloud[i].GetComponent<LineRenderer>();

            line.SetPosition(0, point1);
            line.SetPosition(1, point2);
        }

    }
}
