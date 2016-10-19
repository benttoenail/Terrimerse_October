using UnityEngine;
using System.Collections;

public class MeasureTool : MonoBehaviour {

	public GameObject sphere01;
	public GameObject sphere02;
	public GameObject textPlane;

	public GameObject playerHead;

	Vector3[] points = new Vector3[2];

	LineRenderer line;

	// Use this for initialization
	void Start () {
		
		line = GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pos1 = sphere01.transform.position;
		Vector3 pos2 = sphere02.transform.position;

		points[0] = pos1;
		points[1] = pos2;

		line.SetPositions(points);

		DoTextPlane(pos1, pos2);
	
	}

	void DoTextPlane(Vector3 _pos1, Vector3 _pos2) {

		Vector3 midPoint = (_pos1 + _pos2) / 2;
		Vector3 planePos = new Vector3(midPoint.x, midPoint.y - 1, midPoint.z);

		textPlane.transform.position = planePos;

		textPlane.transform.LookAt(Camera.main.transform.position, transform.up);
		textPlane.transform.Rotate(0, 180, 0);

		float dist = Vector3.Distance(_pos1, _pos2);
		string distance = dist.ToString();

		textPlane.GetComponent<TextMesh>().text = distance;

	}
		
}
