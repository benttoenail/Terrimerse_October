using UnityEngine;
using System.Collections;

public class CamRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 mouse = Input.mousePosition;
		mouse.z = 10;
	
		if(Input.GetKey(KeyCode.A)){
			this.transform.Rotate(0, 2, 0);
		}
		if(Input.GetKey(KeyCode.D)){
			this.transform.Rotate(0, -2, 0);
		}
		if(Input.GetKey(KeyCode.W)){
			this.transform.Rotate(2, 0, 0);
		}
		if(Input.GetKey(KeyCode.S)){
			this.transform.Rotate(-2, 0, 0);
		}

		if(Input.GetKey(KeyCode.Q)){
			this.transform.Translate(0, 0, 5f);
		}
		if(Input.GetKey(KeyCode.E)){
			this.transform.Translate(0, 0, -5f);
		}

	}
}
