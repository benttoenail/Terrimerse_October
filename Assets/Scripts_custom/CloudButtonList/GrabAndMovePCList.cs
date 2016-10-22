using UnityEngine;
using System.Collections;
using Valve.VR;

public class GrabAndMovePCList : MonoBehaviour {

    VRMenuButton myButton;
    public GameObject PCListHandler;
    SteamVR_Controller.Device device;

    GameObject controller;
    bool isMoving;

    Vector3 PClistOrigin;
    // Use this for initialization
    void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += MovePCList;
	}
	
	// Update is called once per frame
	void Update () {

        if (isMoving)
        {
            PCListHandler.transform.position = PClistOrigin - controller.transform.position *-1;
        }
	}


    void MovePCList(VRMenuEventData e)
    {
        PClistOrigin = transform.position.normalized;
        controller = e.originator;
        isMoving = true;
    }
}
