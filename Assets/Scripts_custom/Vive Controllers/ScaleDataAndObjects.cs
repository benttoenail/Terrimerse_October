using UnityEngine;
using System.Collections;
using Valve.VR;

public class ScaleDataAndObjects : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    public GameObject player;
    GameObject pivotPoint;

    Vector3 startingScale;
    Vector3 dataSetScale;
    Vector3 previousControllerPosition;

    float referenceScale;
    float currentScale;
    
    // Use this for initialization
    void Start () {
        //dataSet = GameObject.Find("TestMoveCube!"); // Had to do this to find object...
        //dataSet = GameObject.FindGameObjectWithTag("DataSet");
        player = GameObject.FindGameObjectWithTag("Player");
        pivotPoint = new GameObject();
        pivotPoint.transform.localScale = new Vector3(1, 1, 1);

    }
	
	// Update is called once per frame
	void Update () {

        controller = gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)controller.index);

        //get Starting scale
        //Create New Gameobject
        //Parent object to null
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            referenceScale = player.transform.localScale.x;
            currentScale = referenceScale;

            startingScale = pivotPoint.transform.localScale;
            pivotPoint.transform.position = controller.transform.position;
            //previousControllerPosition = controller.transform.position;
            player.transform.SetParent(pivotPoint.transform);

        }

        //Scale object to controller position
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            // Vector3 delta = controller.transform.position - controllerOrigin;
            Vector3 delta = controller.transform.position - previousControllerPosition;
            float scaleValue = Vector3.Dot(delta, controller.transform.forward) * 5;
            //float scaleValue = delta.x + delta.z;
            //float scaleMultiplier = player.transform.localScale.x * 0.01f;

            currentScale += scaleValue;
            pivotPoint.transform.localScale = startingScale * (currentScale / referenceScale) ;
            /*
            dataSetScale = new Vector3(scaleValue, scaleValue, scaleValue);
            pivotPoint.transform.localScale = startingScale + dataSetScale / 500.0f * scaleMultiplier;
            //pivotPoint.transform.localScale = Vector3.Scale(startingScale, dataSetScale)/400;
            */
        }

        
        //Unparent the object from the pivot Gameobject
        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            player.transform.SetParent(null);
        }

        previousControllerPosition = controller.transform.position;
    }
}
