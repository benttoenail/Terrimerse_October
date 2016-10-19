using UnityEngine;
using System.Collections;
using Valve.VR;

//Moving the DATASET with the Left Handed Vive Controller\\

public class MoveDataSetControls : MonoBehaviour {

	SteamVR_Controller.Device device;
	SteamVR_TrackedObject controller; // some new changes

	public GameObject dataSet;

	float y;

	Vector3 controllerOrigin;
	Vector3 dataSetOrigin;

	// Use this for initialization
	void Start () {
		controller = gameObject.GetComponent<SteamVR_TrackedObject> ();
	}
	
	// Update is called once per frame
	void Update () {

		device = SteamVR_Controller.Input ((int)controller.index);

		if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)){
			controllerOrigin = transform.position;
			dataSetOrigin = dataSet.transform.position;
		}

		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			Vector3 delta =   transform.position - controllerOrigin;
			dataSet.transform.position = delta + dataSetOrigin;
		}

	}


	//First Implementation of moving the DataSet
	void GlideDataSet(){

		device = SteamVR_Controller.Input ((int)controller.index);

		if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)){

			y = transform.position.y;

		}

		if (device.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {

			float currentY = transform.position.y;
			float deltaY = y - currentY;

			Vector3 dataNewPos = new Vector3 (dataSet.transform.position.x, dataSet.transform.position.y + deltaY * 0.1f, dataSet.transform.position.z);
			dataSet.transform.position = dataNewPos;

		}

	}
}
