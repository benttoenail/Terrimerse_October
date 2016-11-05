using UnityEngine;
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

	const float minDragDistance = 0.5f;

    GameObject toolPrefab;
    Transform sphere02;

	ControllerMenuInteractor interactor; 
	void Start() {
		interactor = transform.parent.GetComponentInChildren<ControllerMenuInteractor> ();
	}

	// Update is called once per frame
	void Update () {
        controller = gameObject.GetComponentInParent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)controller.index);

        triggerHoldDown = device.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        triggerDown = device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        triggerUp = device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);

		DrawTool ();	

    }


    //Drawing measurements
    void DrawTool()
	{
		if(triggerDown) {
			if (interactor.isIntersecting) {
				foreach (VRMenuItem item in interactor.intersectedItems) {
					if (item.tag == "MeasureSphere") {
						sphere02 = item.transform;
						break;
					}
				}
			} else {
				toolPrefab = Instantiate (measureTool, interactor.transform.position, Quaternion.identity) as GameObject;

				toolPrefab.transform.parent = GameObject.FindGameObjectWithTag ("DataSet").transform; //Will have to find the Dataset again!  

				sphere02 = toolPrefab.transform.FindChild ("Sphere_02");

				origin = interactor.transform.position;
			}
		}

        if (triggerHoldDown && sphere02 != null)
        {
            //check to see if Dragging
            float dist = Vector3.Distance(origin, interactor.transform.position);
            if(dist > minDragDistance)
            {
                sphere02.transform.position = interactor.transform.position;
                isDragging = true;
            }
        
		}

        if (triggerUp)
        {
            if(sphere02 != null)
            {
                sphere02.transform.parent = toolPrefab.transform;
                sphere02.gameObject.AddComponent<VRMenuItem>();
                sphere02.gameObject.AddComponent<MeasureObjectControl>();

                if (isDragging || sphere02.transform.parent == interactor)
                {
                    sphere02.GetComponentInParent<MeasureTool>().MakeText();
                    isDragging = false;
                }
            }
            
			sphere02 = null;
        }

    }
}
