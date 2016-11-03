﻿using UnityEngine;
using System.Collections;
using Valve.VR;

public class DrawMeasurement : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject controller;

    //EVENTS
    public delegate void DrawingDone();
    public static event DrawingDone OnDrawDone;

    bool triggerHoldDown = false;
    bool triggerDown = false;
    bool triggerUp = false;
    bool isDragging;
    Vector3 origin;

    public GameObject measureTool;
    public GameObject measureShape;

    GameObject toolPrefab;
    Transform sphere02;

    bool isIntersecting = false;
	
	// Update is called once per frame
	void Update () {

        if (transform.parent.GetComponentInChildren<ControllerMenuInteractor>().isIntersecting)
        {
           // return;
        }
        controller = gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)controller.index);

        triggerHoldDown = device.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        triggerDown = device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        triggerUp = device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);

        print("Intersecting is equal to: " + isIntersecting);

        DrawTool();

        print(isDragging);

    }


    //Drawing measurements
    void DrawTool()
    {
        GameObject interactor = controller.transform.FindChild("Interactor").gameObject;
        

        if (triggerDown)
        {
            toolPrefab = Instantiate(measureTool, interactor.transform.position, Quaternion.identity) as GameObject;

            toolPrefab.transform.parent = GameObject.FindGameObjectWithTag("DataSet").transform; //Will have to find the Dataset again!  

            sphere02 = toolPrefab.transform.FindChild("Sphere_02");

            origin = interactor.transform.position;

        }

        if (triggerHoldDown)
        {
            //check to see if Dragging
            float dist = Vector3.Distance(origin, interactor.transform.position);
            if(dist > 2.5)
            {
                sphere02.transform.position = interactor.transform.position;
                isDragging = true;
            }
            
        }

        if (triggerUp)
        {
            sphere02.transform.parent = toolPrefab.transform;
            sphere02.gameObject.AddComponent<VRMenuButton>();
            sphere02.gameObject.AddComponent<MeasureObjectControl>();

            if (isDragging || sphere02.transform.parent == interactor)
            {
                OnDrawDone();
                isDragging = false;
            }
            
        }

    }


    //Set intersecting bool to "true" when interactor is intersecting
    //Don't draw if interacting with another Measurement tool
    //NOT WORKING!!!
    
    public void IsInSphere()
    {
        //print("Controller has Entered");
        SetIntersectingOn();
       // SetIntersectingOn();
    }

    private void SetIntersectingOn()
    {
        isIntersecting = true;
    }

    public void IsOutSphere()
    {
        //print("Controller has Exited");
        SetIntersectingOff();
        //isIntersecting = false;
    }

    private void SetIntersectingOff()
    {
        isIntersecting = false;
    }
    

}
