using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;


public class CloudBlock : MonoBehaviour {

    TextMesh text;
    public string cloudName;
    GameObject pointCloud;

    bool triggerIsPressed;

    public void GetCloudData(string n, GameObject g)
    {
        cloudName = n;
        pointCloud = g;
    }

	// Use this for initialization
	void Start () {

        ControllerEvents.ControllerTriggerPressed += ControllerTriggerPressed;
        ControllerEvents.ControllerTriggerUp += ControllerTriggerUp;

        text = GetComponent<TextMesh>();
        text.text = cloudName;
        print("This Button is for: " + pointCloud.name);

    }

    // Update is called once per frame
    void Update()
    {
        //print(triggerIsPressed);
    }



    public void ControllerTriggerPressed()
    {
        triggerIsPressed = true;
    }

    public void ControllerTriggerUp()
    {
        triggerIsPressed = false;
    }
}
