using UnityEngine;
using System.Collections;
using Valve.VR;

//Moving the DATASET with the Left Handed Vive Controller\\

public class MoveDataSetControls : ControllerFunctionality {

	SteamVR_Controller.Device device;
	SteamVR_TrackedObject controller; // some new changes

	public GameObject dataSet;

	float y;

	Vector3 controllerOrigin;
	Vector3 dataSetOrigin;

    bool triggerHoldDown = false;
    bool triggerDown = false;
    bool triggerUp = false;

    ControllerMenuInteractor interactor;

    // Use this for initialization
    void Start () {
        dataSet = GameObject.FindGameObjectWithTag("DataSet");
        interactor = transform.parent.GetComponentInChildren<ControllerMenuInteractor>();
    }

	// Update is called once per frame
	public override void HandleInput () {

        controller = gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input ((int)controller.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)){
           
            //controllerOrigin = transform.position;
            controllerOrigin = controller.transform.position;
            dataSetOrigin = dataSet.transform.position;
		}

		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
            Vector3 delta =   controller.transform.position - controllerOrigin;
			dataSet.transform.position = delta + dataSetOrigin;
            isPerformingAction = true;
		}
        else
        {
            isPerformingAction = false;
        }

	}

}
