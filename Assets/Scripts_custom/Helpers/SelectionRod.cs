using UnityEngine;
using System.Collections;

public class SelectionRod : MonoBehaviour {

	public GameObject viveControllerR;
	public GameObject viveControllerL;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		//Parent to the right controller
		transform.parent = viveControllerR.transform;

		Vector3 viveR = viveControllerR.transform.position;
		Vector3 viveL = viveControllerL.transform.position;

		float controllerDistance = Vector3.Distance (viveR, viveL);



		if (controllerDistance < 200.0f) {

			//divide by the scale factor of Vive Camera Rig
			this.transform.localScale = new Vector3 (1, 1, 1)/275;
		} else {
			this.transform.localScale = new Vector3(1, 0, 1)/275;
		}

	}
}
