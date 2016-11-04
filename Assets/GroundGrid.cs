using UnityEngine;
using System.Collections;

public class GroundGrid : MonoBehaviour {

    LineRenderer line;
    Vector3[] points = new Vector3[8];

    public float lineLength;
	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(8);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 startPos = transform.position;

        points[0] = startPos;
        points[1] = startPos + new Vector3(0, 0, lineLength);

        
        points[2] = startPos;
        points[3] = startPos + new Vector3(lineLength, 0, 0);

        
        points[4] = startPos;
        points[5] = startPos + new Vector3(0, 0, -lineLength);

        points[6] = startPos;
        points[7] = startPos + new Vector3(-lineLength, 0, 0);
        

        line.SetPositions(points);
    }
}
