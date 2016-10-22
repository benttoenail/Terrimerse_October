using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class SpectatorController : NetworkBehaviour {
	
	public Camera cam;
	public GameObject menuBubble;
	public GameObject reticle;

	public float flySpeed = 10.0f;
	public string displayName = "";

	private SpectatorControls controls;
	public ViveNetworkFunctions networkFunctions;


	// Use this for initialization
	void Start () {

		controls = new SpectatorHoverControls(this);
		networkFunctions = GetComponent<ViveNetworkFunctions> ();
	}

	private float ax=0;

	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}
		// temp mouse controls
		cam.transform.RotateAround(cam.transform.position,Vector3.up,10*Input.GetAxis("Mouse X"));
		cam.transform.RotateAround(cam.transform.position,cam.transform.right,10*Input.GetAxis("Mouse Y"));

		controls.HandleInput ();
	}

	public void SetControls(SpectatorControls nextControls) {
		controls = nextControls;
	}

	public void TransitionControls(SpectatorControls nextControls) {
		controls.Transition(nextControls);
	}
}