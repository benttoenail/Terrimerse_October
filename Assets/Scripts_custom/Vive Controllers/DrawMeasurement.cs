using UnityEngine;
using System.Collections;
using Valve.VR;
using System.Collections.Generic;

public class DrawMeasurement : ControllerFunctionality {
    const float deleteTimeThreshold = 0.5f;
    const float deleteDistThreshold = 5.0f;
    float dragTime = 0.0f;

    //EVENTS
    public delegate void DrawingDone();
    public static event DrawingDone OnDrawDone;

    bool triggerHoldDown = false;
    bool triggerDown = false;
    bool triggerUp = false;
    Vector3 origin;

    public GameObject measureToolPrefab;
    public GameObject measureShape;

    GameObject measureToolInstance;
    Transform sphere02;

    List<MeasureObjectControl> intersectedSpheres = new List<MeasureObjectControl>();

	protected override void Start() {
		base.Start ();
		interactor = transform.parent.GetComponentInChildren<ControllerMenuInteractor> ();
	}

	// Update is called once per frame
	public override void HandleInput () {
		base.HandleInput ();
        
        triggerHoldDown = device.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        triggerDown = device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        triggerUp = device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);

        DrawTool ();

    }

    public void NotifySphereEnter(MeasureObjectControl sphere)
    {
        if (!intersectedSpheres.Contains(sphere))
        {
            intersectedSpheres.Add(sphere);
        }
    }
    public void NotifySphereExit(MeasureObjectControl sphere)
    {
        if (!isPerformingAction)
        {
            intersectedSpheres.Remove(sphere);
        }
    }


    //Drawing measurements
    void DrawTool()
	{
		if(triggerDown) {
			if (intersectedSpheres.Count == 0) {
				measureToolInstance = Instantiate (measureToolPrefab, interactor.transform.position, Quaternion.identity) as GameObject;

				measureToolInstance.transform.parent = GameObject.FindGameObjectWithTag ("DataSet").transform; //Will have to find the Dataset again!  
                 
				sphere02 = measureToolInstance.transform.Find ("Sphere_02");

            }
            origin = interactor.transform.position;
            isPerformingAction = true;
            dragTime = 0.0f;

		}

        if (triggerHoldDown)
        {
            foreach (MeasureObjectControl sphere in intersectedSpheres)
            {
                sphere.transform.position = interactor.transform.position;
                sphere.GetComponentInParent<MeasureTool>().MakeText();
            }
            if (sphere02 != null)
            {
                sphere02.transform.position = interactor.transform.position;
            }
            dragTime += Time.deltaTime;
        }

        if (triggerUp)
        {
            if(sphere02 != null)
            {
                measureToolInstance.transform.Find("Sphere_01").gameObject.AddComponent<MeasureObjectControl>();
                sphere02.transform.parent = measureToolInstance.transform;
                sphere02.gameObject.AddComponent<MeasureObjectControl>();
                sphere02.GetComponentInParent<MeasureTool>().MakeText();
                sphere02 = null;
                measureToolInstance = null;
            }

            if(dragTime < deleteTimeThreshold && Vector3.Distance(origin,interactor.transform.position) < deleteDistThreshold)
            {
                for(int i = intersectedSpheres.Count - 1; i >= 0; i --)
                {
                    Destroy(intersectedSpheres[i].transform.parent.gameObject);
                }
                intersectedSpheres.Clear();
            }

            isPerformingAction = false;
        }

    }
}
