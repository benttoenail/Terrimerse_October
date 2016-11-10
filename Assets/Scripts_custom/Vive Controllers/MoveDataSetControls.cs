using UnityEngine;
using System.Collections;
using Valve.VR;

//Moving the DATASET with the Left Handed Vive Controller\\

public class MoveDataSetControls : ControllerFunctionality {

	public GameObject dataSet;

	float y;

	Vector3 controllerOrigin;
	Vector3 dataSetOrigin;

    bool triggerHoldDown = false;
    bool triggerDown = false;
    bool triggerUp = false;

    // Use this for initialization
    void Start () {
		base.Start ();
        dataSet = GameObject.FindGameObjectWithTag("DataSet");
    }

	// Update is called once per frame
	public override void HandleInput () {
		base.HandleInput ();

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
