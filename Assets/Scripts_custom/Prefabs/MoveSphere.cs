using UnityEngine;
using System.Collections;

public class MoveSphere : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;




	void OnMouseDown() {
		


		screenPoint = Camera.main.ScreenToWorldPoint(gameObject.transform.position);
		offset = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

	}

	void OnMouseDrag() {


		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

		gameObject.transform.position = curPosition;



//		float distanceToScreen = Camera.main.world

	}
}
