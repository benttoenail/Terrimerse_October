using UnityEngine;
using System.Collections;

public class ScaleCameraRig : MonoBehaviour {

	public GameObject controllerRight;
    public GameObject controllerLeft;

	bool leftPressed;
	bool rightPressed;
	bool leftPressedDown;
	bool rightPressedDown;

	float d;

	// Use this for initialization
	void Start () {
		
		//Get controllers from Controller manager script
		controllerRight = gameObject.GetComponent<SteamVR_ControllerManager> ().right;
		controllerLeft = gameObject.GetComponent<SteamVR_ControllerManager> ().left;


	}
	
	// Update is called once per frame
	void Update () {

		//Get the common bool within each controller's script - Continous press
		leftPressed = controllerLeft.GetComponent<ScaleSceneControls> ().touchPadPressed;
		rightPressed = controllerRight.GetComponent<ScaleSceneControls> ().touchPadPressed;
		//Quick Press
		rightPressedDown = controllerRight.GetComponent<ScaleSceneControls> ().touchPadPressedDown;
		leftPressedDown = controllerRight.GetComponent<ScaleSceneControls> ().touchPadPressedDown;

		Vector3 rightPos = controllerRight.transform.position;
		Vector3 leftPos = controllerLeft.transform.position;

		float distance = Vector3.Distance (rightPos, leftPos);

		Vector3 currentScale = transform.localScale;

		//Get starting value
		if (leftPressedDown && rightPressedDown) {
			d = distance;
		}
		//update scale based on distance
		if (leftPressed && rightPressed) {
			
			float delta = d - distance;
			transform.localScale = new Vector3 (currentScale.x + delta, currentScale.y + delta, currentScale.z + delta);
            

            print (delta);
		}
	
	}
}
