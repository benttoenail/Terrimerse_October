using UnityEngine;
using System.Collections;
using Valve.VR;

public class RotateDataSet : ControllerFunctionality {

    public GameObject dataSet;
    public float rotationSpeed = 0.5f;

    Vector3 originalRotation;
    Vector3 dataSetRotation;
    Vector3 controllerOrigin;

    bool triggerHoldDown = false;
    bool triggerDown = false;
    bool triggerUp = false;

    // Use this for initialization
    void Start () {
		base.Start ();
        dataSet = GameObject.FindGameObjectWithTag("DataSet");
    }

    // Update is called once per frame
    public override void HandleInput()
    {
		base.HandleInput ();

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
            dataSet.transform.eulerAngles = originalRotation + dataSetRotation / 2;
            isPerformingAction = true;

        }else
        {
            isPerformingAction = false;
        }
    

    }
}
