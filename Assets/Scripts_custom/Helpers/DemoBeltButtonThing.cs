using UnityEngine;
using System.Collections;

public class DemoBeltButtonThing : MonoBehaviour {
	VRMenuButton myButton;
	public GameObject myPrefab; //Prefab Gameobject with toolScript attached
    HoverBeltItems HoverBelt;

    public Mesh mesh;

	// Use this for initialization
	void Start () {
		myButton = GetComponent<VRMenuButton> ();
		myButton.OnClick += DoStuff;

        HoverBelt = GetComponentInParent<HoverBeltItems>();
	}

	void DoStuff(VRMenuEventData e) {
		// This is a SteamVR_TrackedObject
		GameObject controllerObj = e.originator;

		// Example of getting the actual component that created this event
		ControllerMenuInteractor actualController = controllerObj.GetComponentInChildren<ControllerMenuInteractor> ();

        //Update ToolTracker
        ToolTracker toolTracker = controllerObj.GetComponentInChildren<ToolTracker>();
        toolTracker.UpdateCurrentTool(myPrefab, mesh);

        HoverBelt.DoClose();
	}

}