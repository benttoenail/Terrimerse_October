using UnityEngine;
using System.Collections;
using Valve.VR;

public class ScaleDataAndObjects : ControllerFunctionality {

    public GameObject player;
    GameObject pivotPoint;

    Vector3 startingScale;
    Vector3 dataSetScale;
    Vector3 previousControllerPosition;

    float referenceScale;
    float currentScale;

    public static bool isScaling = false;
    GameObject dataSet;
    MeasureTool measureTool;

    bool triggerHoldDown = false;
    bool triggerDown = false;
    bool triggerUp = false;

    // Use this for initialization
    void Start () {
		base.Start ();

        player = GameObject.FindGameObjectWithTag("Player");
        pivotPoint = new GameObject();
        pivotPoint.transform.localScale = new Vector3(1, 1, 1);
        dataSet = GameObject.FindGameObjectWithTag("DataSet");

    }

    // Update is called once per frame
    public override void HandleInput()
    {
		base.HandleInput ();

        //get Starting scale
        //Create New Gameobject
        //Parent object to null
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            referenceScale = player.transform.localScale.x;
            currentScale = referenceScale;

            startingScale = pivotPoint.transform.localScale;
            pivotPoint.transform.position = controller.transform.position;
            //player.transform.SetParent(pivotPoint.transform);
            dataSet.transform.SetParent(pivotPoint.transform);
            isPerformingAction = true;
        }

        //Scale object to controller position
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            isScaling = true;
            Vector3 delta = controller.transform.position - previousControllerPosition;
            float scaleValue = Vector3.Dot(delta, controller.transform.forward) * 5;

            currentScale += scaleValue;
            pivotPoint.transform.localScale = startingScale * (currentScale / referenceScale) ;

            interactor.ignoreMenus = true;

        }

        
        //Unparent the object from the pivot Gameobject
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            isScaling = false;
            //player.transform.SetParent(null);
            dataSet.transform.SetParent(null);

            interactor.ignoreMenus = false;
            isPerformingAction = false;
        }

        previousControllerPosition = controller.transform.position;
    }

	void OnDestroy() {
		isScaling = false;
	}

}
