using UnityEngine;
using System.Collections;
using Valve.VR;

public class RotateDataSet : ControllerFunctionality {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    public GameObject dataSet;
    public float rotationSpeed = 0.5f;

    Vector3 originalRotation;
    Vector3 dataSetRotation;
    Vector3 controllerOrigin;

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
    public override void HandleInput()
    {

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
            dataSet.transform.eulerAngles = originalRotation + dataSetRotation / 2;
            isPerformingAction = true;

        }else
        {
            isPerformingAction = false;
        }
    

    }
}
