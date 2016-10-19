﻿using UnityEngine;
using System.Collections;
using Valve.VR;

public class DrawMeasurement : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    bool triggerHoldDown = false;
    bool triggerDown = false;
    bool triggerUp = false;

    public GameObject measureTool;

    GameObject toolPrefab;
    Transform sphere02;

    bool isIntersecting;

    // Use this for initialization
    void Start () {

        controller = gameObject.GetComponent<SteamVR_TrackedObject>();

    }
	
	// Update is called once per frame
	void Update () {

        device = SteamVR_Controller.Input((int)controller.index);

        triggerHoldDown = device.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        triggerDown = device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        triggerUp = device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);

        DrawTool();

    }


    void DrawTool()
    {
        if (triggerDown && !isIntersecting)
        {
            toolPrefab = Instantiate(measureTool, controller.transform.position, Quaternion.identity) as GameObject;

            //int childCount = DataSet.transform.childCount;
            toolPrefab.transform.parent = GameObject.Find("DataSetCube").transform;

            sphere02 = toolPrefab.transform.FindChild("Sphere_02");
            sphere02.transform.parent = controller.transform;
        }

        if (triggerUp)
        {
            sphere02.transform.parent = toolPrefab.transform;
        }

    }

    void OnTriggerEnter()
    {
        isIntersecting = true;
    }

    void OnTriggerExit()
    {
        isIntersecting = false;
    }
}
