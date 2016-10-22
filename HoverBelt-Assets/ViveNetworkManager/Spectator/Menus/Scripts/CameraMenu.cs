using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CameraMenu : MonoBehaviour {
	public VRMenuTextInput flightSpeedField;
	public VRMenuButton followButton;
	public Text avatarCountText;
	public SpectatorController controller;

	private int avatarCount = 0;

	// Use this for initialization
	void Start () {
		if (controller == null) {
			controller = GetComponentInParent<SpectatorController> ();
		}
		flightSpeedField.SetInitialValue(controller.flySpeed.ToString ());

		flightSpeedField.OnValueSet += SetFlightSpeed;
		followButton.OnClick += EnterFollowMode;

		avatarCountText.text = avatarCount.ToString();
	}

	void SetFlightSpeed(string s) {
		try {
			controller.flySpeed = float.Parse (s);
		}
		catch (Exception e) {

		}
	}

	void EnterFollowMode(){
		controller.TransitionControls (new SpectatorFollowControls (controller));
	}

	// Update is called once per frame
	void Update () {

		// Inefficient
		int _avatarCount = ViveAvatar.avatars.Count;
		if(avatarCount != _avatarCount) {
			avatarCount = _avatarCount;
			avatarCountText.text = avatarCount.ToString ();
		}
	}

}
