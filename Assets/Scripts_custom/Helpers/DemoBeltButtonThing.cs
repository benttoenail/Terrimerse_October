using UnityEngine;
using System.Collections;

public class DemoBeltButtonThing : MonoBehaviour {
	VRMenuButton myButton;
	public GameObject myPrefab; //Prefab Gameobject with toolScript attached

	// Use this for initialization
	void Start () {
		myButton = GetComponent<VRMenuButton> ();
		myButton.OnClick += DoStuff;
	}

	void DoStuff(VRMenuEventData e) {
		// This is a SteamVR_TrackedObject
		GameObject controllerObj = e.originator;

		// Example of getting the actual component that created this event
		ControllerMenuInteractor actualController = controllerObj.GetComponentInChildren<ControllerMenuInteractor> ();

        /*
		// Attach something to the controller
		GameObject anInstance = (GameObject) Instantiate (myPrefab);

		anInstance.transform.SetParent (controllerObj.transform, false);
        */

        //Update ToolTracker
        ToolTracker toolTracker = controllerObj.GetComponentInChildren<ToolTracker>();
        toolTracker.UpdateCurrentTool(myPrefab);
	}

}