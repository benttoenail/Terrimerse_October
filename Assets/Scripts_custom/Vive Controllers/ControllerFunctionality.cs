using UnityEngine;
using System.Collections;

public abstract class ControllerFunctionality : MonoBehaviour {

	protected ControllerMenuInteractor interactor;
	protected SteamVR_Controller.Device device;
	protected SteamVR_TrackedObject controller;

    public bool isPerformingAction = false;

	protected virtual void Start() {
		interactor = transform.parent.GetComponentInChildren<ControllerMenuInteractor> ();
	}

	public virtual void HandleInput() {

		controller = gameObject.GetComponentInParent<SteamVR_TrackedObject>();
		device = SteamVR_Controller.Input((int)controller.index);
	}
}
