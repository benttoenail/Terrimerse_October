using UnityEngine;
using System.Collections;
using Valve.VR;

public class RotateDataSet : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    public GameObject dataSet;
    public float rotationSpeed = 100;

    Vector3 originalRotation;
    Vector3 dataSetRotation;
    Vector3 controllerOrigin;

    // Use this for initialization
    void Start () {
        dataSet = GameObject.Find("TestMoveCube!"); // Had to do this to find object...
        //controller = gameObject.GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {

        controller = gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)controller.index);

        dataSetRotation.x = controller.transform.position.x;

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            originalRotation = dataSet.transform.eulerAngles;
            controllerOrigin = controller.transform.position;
        }
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Vector3 delta = controller.transform.position - controllerOrigin;
            dataSetRotation = new Vector3(0, delta.x + delta.z, 0);
            dataSet.transform.eulerAngles = originalRotation + dataSetRotation * rotationSpeed;
        }
    

    }
}
