using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerMenuInteractor : MonoBehaviour {
	
	private List<VRMenuItem> intersectedItems = new List<VRMenuItem>();
	private SteamVR_TrackedObject trackedObj;

	public event VRMenuItem.VRMenuEvent OnEmptyClick;

	public bool isIntersecting {
		get {
			return intersectedItems.Count > 0;
		}
	}


	public void Start() {
		GetComponent<MeshRenderer> ().material.color = new Color (1, 0, 0, 0.25f);
		trackedObj = GetComponentInParent<SteamVR_TrackedObject> ();
	}



	void OnTriggerEnter(Collider other) {
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

	void Update() {
		HandleInput ();
	}

	public virtual void HandleInput() {
		float now = Time.time;

		SteamVR_Controller.Device device =  SteamVR_Controller.Input((int)trackedObj.index);
		CleanList ();

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			HandlePointerDown ();
			prevDown = lastDown;
			lastDown = now;
			isPointerDown = true;
		}

		if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
			HandlePointerUp ();

			// Detect click
			if (now - lastDown <= clickDuration) {

				// Detect double click
				if((lastUp - prevDown <= clickDuration) && (lastDown - lastUp <= doubleClickPauseDuration)) {
					HandleDoubleClick();
				}
				else {
					if (isIntersecting) {
						Debug.Log ("Click");
						HandleClick ();
					} else {
						Debug.Log ("Click empty");
						if (OnEmptyClick != null) {
							OnEmptyClick (VRMenuEventData.FromVive(trackedObj));
						}
					}
				}
			}
			lastUp = now;
			isPointerDown = false;
		}
	}

	/* End click detection code */

	protected void CleanList() {
		for (int i = intersectedItems.Count - 1; i >= 0; i--) {
			if (intersectedItems [i] == null || !intersectedItems[i].gameObject.activeSelf) {
				intersectedItems.RemoveAt (i);
			}
		}
	}

	protected virtual void HandlePointerDown() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyPointerDown (VRMenuEventData.FromVive(trackedObj));
		}
	}
	protected virtual void HandlePointerUp() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyPointerUp (VRMenuEventData.FromVive(trackedObj));
		}
	}
	protected virtual void HandleClick() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyClick (VRMenuEventData.FromVive(trackedObj));
		}
	}
	protected virtual void HandleDoubleClick() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyDoubleClick (VRMenuEventData.FromVive(trackedObj));
		}
	}

}
