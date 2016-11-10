using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ControllerMenuInteractor : MonoBehaviour {

	List<GameObject> blockingItems = new List<GameObject>();
	public List<VRMenuItem> intersectedItems = new List<VRMenuItem>();
	private SteamVR_TrackedObject trackedObj;

    public bool ignoreMenus = false;

	public HoverBeltItems hoverBelt;

	public bool isIntersecting {
		get {
			return intersectedItems.Count > 0;
		}
	}

	public bool isBeltOpen {
		get {
			return hoverBelt != null && hoverBelt.state == HoverBeltItems.BeltState.Open;
		}
	}

    public bool isBlocked
    {
        get
        {
            return blockingItems.Count > 0;
        }
	}

	bool doMenuInteractions
	{
		get {
			return (isIntersecting || isBeltOpen) && (currentFunctionality == null || !currentFunctionality.isPerformingAction) && !isBlocked;
		}
	}


	public void Start() {
		GetComponent<MeshRenderer> ().material.color = new Color (1, 0, 0, 0.25f);
		trackedObj = GetComponentInParent<SteamVR_TrackedObject> ();
	}



	void OnTriggerEnter(Collider other) {
		VRMenuItem item = other.GetComponentInParent<VRMenuItem> ();
		if (item != null) {
			intersectedItems.Add (item);
            item.NotifyIn();

            if (isPointerDown)
            {
                item.NotifyPointerDown();
            }
        }
        GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 0.25f);
    }

	void OnTriggerExit(Collider other) {
		VRMenuItem item = other.GetComponentInParent<VRMenuItem> ();
		if (item != null) {
			intersectedItems.Remove (item);
            if (isPointerDown)
            {
                item.NotifyPointerUp();
            }

            item.NotifyOut();
        }
        if (!isIntersecting)
        {
            GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.25f);
        }
    }

    public void AddBlock(GameObject go)
    {
        blockingItems.Add(go);
        GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 0.25f);

    }

    public void RemoveBlock(GameObject go)
    {
        blockingItems.Remove(go);
        if (!isIntersecting)
        {
            GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0.25f);
        }

    }


    /* Begin click detection code */

    private float prevDown = Mathf.NegativeInfinity;
	private float lastDown = Mathf.NegativeInfinity;
	private float lastUp = Mathf.NegativeInfinity;

	protected bool isPointerDown = false;

	private static readonly float clickDuration = 0.5f;
	private static readonly float doubleClickPauseDuration = 0.2f;

    public ControllerFunctionality currentFunctionality;

	void Update() {
        if(doMenuInteractions)
        {
            HandleInput();
        }
        else if(!isBlocked)
        {
            if (currentFunctionality != null)
            {
                currentFunctionality.HandleInput();
            }
		}

		// Toggle cross section if grip is pressed, regardless of tool status
		if(SteamVR_Controller.Input((int)trackedObj.index).GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			StateManager.singleton.ToggleCrossSection ();
		}
	}

	public virtual void HandleInput() {
		float now = Time.time;

		SteamVR_Controller.Device device =  SteamVR_Controller.Input((int)trackedObj.index);
		CleanList ();

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
			HandlePointerDown ();
			prevDown = lastDown;
			lastDown = now;
			isPointerDown = true;
		}

		if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)) {
			HandlePointerUp ();

			// Detect click
			if (now - lastDown <= clickDuration) {

				// Detect double click
				if((lastUp - prevDown <= clickDuration) && (lastDown - lastUp <= doubleClickPauseDuration)) {
					HandleDoubleClick();
				}
				else {
					if (isIntersecting) {
						HandleClick ();
					} else {
						HandleEmptyClick ();
					}
				}
			}
			lastUp = now;
			isPointerDown = false;
		}
	}

	/* End click detection code */

	protected void CleanList() {
		for (int i = intersectedItems.Count - 1; i >= 0; i--) {
			if (intersectedItems [i] == null || !intersectedItems[i].gameObject.activeSelf) {
				intersectedItems.RemoveAt (i);
			}
		}
		for (int i = blockingItems.Count - 1; i >= 0; i--) {
			if (blockingItems [i] == null || !blockingItems[i].gameObject.activeSelf) {
				blockingItems.RemoveAt (i);
			}
		}
	}

	protected virtual void HandlePointerDown() {
        foreach (VRMenuItem item in intersectedItems) {
			item.NotifyPointerDown (VRMenuEventData.FromVive(trackedObj));
		}
	}
	protected virtual void HandlePointerUp() {
        foreach (VRMenuItem item in intersectedItems) {
        if (ignoreMenus) { return; }
			item.NotifyPointerUp (VRMenuEventData.FromVive(trackedObj));
		}
	}
	protected virtual void HandleClick() {
		foreach (VRMenuItem item in intersectedItems) {
			item.NotifyClick (VRMenuEventData.FromVive(trackedObj));
		}
	}
	protected virtual void HandleDoubleClick()
    {
        foreach (VRMenuItem item in intersectedItems) {
			item.NotifyDoubleClick (VRMenuEventData.FromVive(trackedObj));
		}
	}
	protected virtual void HandleEmptyClick() {
		if (isBeltOpen) {
			hoverBelt.DoClose ();
		}
	}

}
