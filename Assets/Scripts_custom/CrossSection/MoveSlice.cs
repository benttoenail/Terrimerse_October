using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveSlice : MonoBehaviour {
    Transform interactor;

    List<SteamVR_TrackedObject> objectsIntersecting = new List<SteamVR_TrackedObject>();
    List<GrabDescriptor> devicesGrabbedBy = new List<GrabDescriptor>();

    class GrabDescriptor
    {
        public SteamVR_TrackedObject to;
        public SteamVR_Controller.Device device;
        public Vector3 prevPos;

        public GrabDescriptor(SteamVR_TrackedObject _to, SteamVR_Controller.Device _device)
        {
            to = _to;
            device = _device;
            prevPos = to.transform.position;
        }
    }

    public GameObject crossSection;
    Vector3 oriPos;

	updateplane planeComponent;
    void Start()
    {
        oriPos = transform.position;
		planeComponent = GetComponent<updateplane> ();
    }


    // Update is called once per frame
    void Update () {

        UpdateGrabStatus();

        UpdateGrabMovement();

	}

    void UpdateGrabStatus()
    {
        // Check for grab start
        foreach (SteamVR_TrackedObject to in objectsIntersecting)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)to.index);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                //to.GetComponentInChildren<ToolTracker>().DeActivateTool(); Commented out to temp. fix a glitch
                to.GetComponentInChildren<ControllerMenuInteractor>().AddBlock(gameObject);
                devicesGrabbedBy.Add(new GrabDescriptor(to, device));
            }
        }

        // Check for grab stop
        for (int i = devicesGrabbedBy.Count - 1; i >= 0; i--)
        {
			if (devicesGrabbedBy[i].device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                //devicesGrabbedBy[i].to.GetComponentInChildren<ToolTracker>().ActivateTool(); Commented out to temp. fix a glitch
                devicesGrabbedBy[i].to.GetComponentInChildren<ControllerMenuInteractor>().RemoveBlock(gameObject);
                devicesGrabbedBy.RemoveAt(i);
            }
        }
    }

	void OnDisable() {
		// When this object is set inactive, clear its grab status
		foreach (GrabDescriptor desc in devicesGrabbedBy)
		{
			//desc.to.GetComponentInChildren<ToolTracker>().ActivateTool();
			desc.to.GetComponentInChildren<ControllerMenuInteractor>().RemoveBlock(gameObject);
		}
		devicesGrabbedBy.Clear ();
		objectsIntersecting.Clear ();
	}

    void UpdateGrabMovement()
    {

        foreach (GrabDescriptor grab in devicesGrabbedBy)
        {
            // Do movement
            Vector3 delta = grab.to.transform.position - grab.prevPos;

            transform.position += transform.up * Vector3.Dot(transform.up, delta);

            // Update previous position
            grab.prevPos = grab.to.transform.position;

			if (planeComponent != null) {
				planeComponent.wasMoved = true;
			}
        }


    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Interactor")
        {
            SteamVR_TrackedObject trackedObject = col.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
            objectsIntersecting.Add(trackedObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Interactor")
        {
            SteamVR_TrackedObject trackedObject = col.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
            objectsIntersecting.Remove(trackedObject);
        }
    }

    //Call From StateManager
    public void MoveToOriginalPosition()
    {
        //transform.position = oriPos;
        if (!crossSection.activeSelf)
        {
            transform.position = oriPos;
        }else
        {
            StartCoroutine(LerpSlice(0.4f));
        }
        
    }

    IEnumerator LerpSlice(float time)
    {
        float elapsedTime = 0;

        while(elapsedTime < time)
        {
            transform.position = Vector3.Lerp(transform.position, oriPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }

}
