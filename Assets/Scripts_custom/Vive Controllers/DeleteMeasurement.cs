using UnityEngine;
using System.Collections;
using Valve.VR;

public class DeleteMeasurement : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    GameObject measureTool;

    bool collideMeasure;

    // Use this for initialization
    void Start () {

        controller = gameObject.GetComponent<SteamVR_TrackedObject>();

    }
	
	// Update is called once per frame
	void Update () {

        device = SteamVR_Controller.Input((int)controller.index);

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && collideMeasure)
        {
            if(measureTool != null)
            {
                Destroy(measureTool);
            }
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "MeasureSphere")
        {
            collideMeasure = true;
            measureTool = col.gameObject.transform.parent.gameObject;
        }

    }

    void OnTriggerExit(Collider col)
    {
        collideMeasure = false;
    }
}
