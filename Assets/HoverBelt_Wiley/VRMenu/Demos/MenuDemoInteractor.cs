using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuDemoInteractor : MonoBehaviour {
	private MenuDemoPlayer player;
	private ViveController controller;

	private List<VRMenuItem> intersectedItems = new List<VRMenuItem>();

	public bool isIntersecting {
		get {
			return intersectedItems.Count > 0;
		}
	}


	public void Configure(MenuDemoPlayer _player, ViveController _controller) {
		player = _player;
		controller = _controller;
		GetComponent<MeshRenderer> ().material.color = new Color (1, 0, 0, 0.25f);
	}



	void OnTriggerEnter(Collider other) {
		Debug.Log ("A");
		VRMenuItem item = other.GetComponentInParent<VRMenuItem> ();
		if (item != null) {
			intersectedItems.Add (item);
			item.NotifyIn ();

			if (isPointerDown) {
				item.NotifyPointerDown ();
			}
		}
		GetComponent<MeshRenderer> ().material.color = new Color (0, 1, 0, 0.25f);
	}

	void OnTriggerExit(Collider other) {
		VRMenuItem item = other.GetComponentInParent<VRMenuItem> ();
		if (item != null) {
			intersectedItems.Remove (item);

			if (isPointerDown) {
				item.NotifyPointerUp ();
			}

			item.NotifyOut ();
		}
		if (!isIntersecting) {
			GetComponent<MeshRenderer> ().material.color = new Color (1, 0, 0, 0.25f);
		}
	}


	/* Begin click detection code */

	private float prevDown = Mathf.NegativeInfinity;
	private float lastDown = Mathf.NegativeInfinity;
	private float lastUp = Mathf.NegativeInfinity;

	protected bool isPointerDown = false;

	private static readonly float clickDuration = 0.5f;
	private static readonly float doubleClickPauseDuration = 0.2f;

	public virtual void HandleInput() {
		float now = Time.time;

		CleanList ();

		if (controller.localDevice.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			HandlePointerDown ();
			prevDown = lastDown;
			lastDown = now;
			isPointerDown = true;
		}

		if (controller.localDevice.GetPressUp (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			HandlePointerUp ();

			// Detect click
			if (now - lastDown <= clickDuration) {

				// Detect double click
				if((lastUp - prevDown <= clickDuration) && (lastDown - lastUp <= doubleClickPauseDuration)) {
					HandleDoubleClick();
				}
				else {
					HandleClick ();
				}
			}
			lastUp = now;
			isPointerDown = false;
		}
	}

	/* End click detection code */

	protected void CleanList() {
		for (int i = intersectedItems.Count - 1; i >= 0; i--) {
			if (intersectedItems [i] == null) {
				intersectedItems.RemoveAt (i);
			}
		}
	}

	protected virtual void HandlePointerDown() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyPointerDown ();
		}
	}
	protected virtual void HandlePointerUp() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyPointerUp ();
		}
	}
	protected virtual void HandleClick() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyClick ();
		}
	}
	protected virtual void HandleDoubleClick() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyDoubleClick ();
		}
	}

}
