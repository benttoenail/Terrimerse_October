using UnityEngine;
using System.Collections;
using Valve.VR;

public class ControllerInteractionEvents : MonoBehaviour {
    
    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    string toolName;

    bool controllerIntersected = false;

    MoveDataSetControls moveTool;
    ScaleDataAndObjects scaleTool;
    RotateDataSet rotateTool;
    DrawMeasurement drawTool;
    DeleteMeasurement deleteTool;

    // Use this for initialization
    void Start ()
    {
        controller = gameObject.GetComponent<SteamVR_TrackedObject>();

        moveTool = gameObject.GetComponent<MoveDataSetControls>();
        scaleTool = gameObject.GetComponent<ScaleDataAndObjects>();
        rotateTool = gameObject.GetComponent<RotateDataSet>();
        drawTool = gameObject.GetComponent<DrawMeasurement>();
        deleteTool = gameObject.GetComponent<DeleteMeasurement>();

    }

    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)controller.index);

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && controllerIntersected)
        {

            //ASSIGN TOOL TO CONTROLLER!
            switch (toolName)
            {
                case "MoveTool":
                    Debug.Log("Clicked the moveTool");
                    moveTool.enabled = true;
                    scaleTool.enabled = false;
                    rotateTool.enabled = false;
                    drawTool.enabled = false;
                    deleteTool.enabled = false;
                    break;
                case "ScaleTool":
                    Debug.Log("Clicked the scaleTool");
                    moveTool.enabled = false;
                    scaleTool.enabled = true;
                    rotateTool.enabled = false;
                    drawTool.enabled = false;
                    deleteTool.enabled = false;
                    break;
                case "RotateTool":
                    Debug.Log("Clicked the rotateTool");
                    moveTool.enabled = false;
                    scaleTool.enabled = false;
                    rotateTool.enabled = true;
                    drawTool.enabled = false;
                    deleteTool.enabled = false;
                    break;
                case "MeasurementTool":
                    Debug.Log("Clicked the measurementTool");
                    moveTool.enabled = false;
                    scaleTool.enabled = false;
                    rotateTool.enabled = false;
                    drawTool.enabled = true;
                    deleteTool.enabled = false;
                    break;
                case "DeleteTool":
                    Debug.Log("Clicked the deleteTool");
                    moveTool.enabled = false;
                    scaleTool.enabled = false;
                    rotateTool.enabled = false;
                    drawTool.enabled = false;
                    deleteTool.enabled = true;
                    break;

                default:
                    break;
            }

        }

    }

    void OnTriggerEnter(Collider col)
    {
        controllerIntersected = true;
        toolName = col.gameObject.tag;
    }

    void OnTriggerExit()
    {
        controllerIntersected = false;
    }
}
