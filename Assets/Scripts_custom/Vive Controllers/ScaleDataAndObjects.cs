using UnityEngine;
using System.Collections;
using Valve.VR;

public class ScaleDataAndObjects : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    public GameObject dataSet;
    GameObject pivotPoint;

    Vector3 startingScale;
    Vector3 dataSetScale;
    Vector3 controllerOrigin;

    
    // Use this for initialization
    void Start () {

        controller = gameObject.GetComponent<SteamVR_TrackedObject>();
        pivotPoint = new GameObject();
        pivotPoint.transform.localScale = new Vector3(1, 1, 1);

    }
	
	// Update is called once per frame
	void Update () {

        device = SteamVR_Controller.Input((int)controller.index);

        //get Starting scale
        //Create New Gameobject
        //Parent object to null
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            startingScale = pivotPoint.transform.localScale;
            pivotPoint.transform.position = transform.position;
            controllerOrigin = transform.position;
            dataSet.transform.SetParent(pivotPoint.transform);

        }

        //Scale object to controller position
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            Vector3 delta = transform.position - controllerOrigin;
            float scaleValue = delta.x + delta.z;

            dataSetScale = new Vector3(scaleValue, scaleValue, scaleValue);
            pivotPoint.transform.localScale = startingScale + dataSetScale;

        }

        //Unparent the object from the pivot Gameobject
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            dataSet.transform.SetParent(null);
        }

    }
}
