using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveControls : MonoBehaviour {

    private SteamVR_Controller.Device controller { get{ return SteamVR_Controller.Input((int)trackedObj.index); } }
    SteamVR_TrackedObject trackedObj;

    public bool triggerHoldDown = false;
    public bool triggerDown = false;
    public bool triggerUp = false;

    bool intersectedOne;
    bool intersectedTwo;
    bool intersectedThree;
	bool intersectedFour;
	bool intersectedFive;

    public GameObject toolUIOne;
    public GameObject toolUITwo;
    public GameObject toolUIThree;
	public GameObject toolUIFour;
	public GameObject toolUIFive;

    GameObject toolPrefab;
    Transform sphere02;

    public GameObject measureTool;

	Transform DataSet;
	GameObject dataSubSet;

	// Use this for initialization
	void Start () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();

		DataSet = GameObject.Find ("DataSet-2-Subsets").transform;
 
	}
	
	// Update is called once per frame
	void Update () {

        //set bools for vive trigger controls
        triggerHoldDown = controller.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        triggerDown = controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        triggerUp = controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);

        //find intersected UI controls -- kinda hacky, can be optimized
		intersectedOne = toolUIOne.gameObject.GetComponent<SubSetUIControls>().intersected;
		intersectedTwo = toolUITwo.gameObject.GetComponent<SubSetUIControls>().intersected;
		intersectedThree = toolUIThree.gameObject.GetComponent<SubSetUIControls>().intersected;
		intersectedFour = toolUIFour.gameObject.GetComponent<SubSetUIControls>().intersected;
		intersectedFive = toolUIFive.gameObject.GetComponent<SubSetUIControls>().intersected;

		/*
		for (int i = 0; i < DataSet.transform.childCount; i++) {
			
			if (DataSet.GetChild [i].activeSelf == true) {
				//dataSubSet == DataSet.transform.GetChild [i];
				print (dataSubSet.gameObject.name);
			}

		}
*/


        DrawTool();

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(trackedObj.transform.position, forward, Color.green);

	}


	//Pass in the object that the measure tool will be parented to
    void DrawTool()
    {
		if (triggerDown && !intersectedOne && !intersectedTwo && !intersectedThree && !intersectedFour && !intersectedFive)
        {
            toolPrefab = Instantiate(measureTool, trackedObj.transform.position, Quaternion.identity) as GameObject;

			//int childCount = DataSet.transform.childCount;
			toolPrefab.transform.parent = GameObject.Find ("DataSet-2-Subsets").transform;

            sphere02 = toolPrefab.transform.FindChild("Sphere_02");
            sphere02.transform.parent = trackedObj.transform;
        }

        if (triggerUp)
        {
            sphere02.transform.parent = toolPrefab.transform;
        }

    }
}
