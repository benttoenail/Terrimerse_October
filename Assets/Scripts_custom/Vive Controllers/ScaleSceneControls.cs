using UnityEngine;
using System.Collections;
using Valve.VR;

public class ScaleSceneControls : MonoBehaviour {

	SteamVR_Controller.Device device;
	SteamVR_TrackedObject controller;

	public bool touchPadPressed = false;
	public bool touchPadPressedDown = false;

	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<SteamVR_TrackedObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		device = SteamVR_Controller.Input ((int)controller.index);
	
		if (device.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			touchPadPressed = true;
		} else {
			touchPadPressed = false;
		}

		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			touchPadPressedDown= true;
		} else {
			touchPadPressedDown = false;
		}
	}
}
