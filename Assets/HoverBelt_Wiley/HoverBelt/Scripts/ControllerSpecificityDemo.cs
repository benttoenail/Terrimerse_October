using UnityEngine;
using System.Collections;

public class ControllerSpecificityDemo : MonoBehaviour {
	VRMenuButton theButton;
	public GameObject theModel;


	void Start () {
		theButton = GetComponentInChildren<VRMenuButton> ();
		theModel = theButton.modelRenderer.gameObject;
		theButton.OnClick += Assign;
	}

	void Assign(VRMenuEventData e) {
		GameObject theModelCopy = (GameObject)Instantiate (theModel);
		theModelCopy.transform.localScale = new Vector3 (0.004f, 0.004f, 0.004f);
		theModelCopy.transform.SetParent (e.originator.transform, false);
	}
}
