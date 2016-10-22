using UnityEngine;
using System.Collections;

public class HoverBeltDemoPlayer : MonoBehaviour {
	public GameObject hoverBeltPrefab;
	public SteamVR_TrackedObject leftController;
	public SteamVR_TrackedObject rightController;
	public Camera headCam;
    public GameObject playerParent;

	// Use this for initialization
	void Start () {
		GameObject hoverBeltObj = (GameObject)Instantiate (hoverBeltPrefab);
		hoverBeltObj.GetComponentInChildren<HoverBeltItems> ().Configure (headCam.gameObject, playerParent.gameObject, leftController.gameObject, rightController.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
