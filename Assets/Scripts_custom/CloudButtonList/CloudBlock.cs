using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;


public class CloudBlock : MonoBehaviour {

    TextMesh text;
    public string cloudName;
    GameObject pointCloud;

    bool triggerIsPressed;
    bool cloudIsActive = true;

    Color activeColor = new Color(255, 0, 0);//red
    Color inactiveColor = new Color(100, 100, 100);//grey

    Vector3 originalScale;

    public void GetCloudData(string n, GameObject g)
    {
        cloudName = n;
        pointCloud = g;
    }

	// Use this for initialization
	void Start () {

        originalScale = transform.localScale;

        ControllerEvents.ControllerTriggerPressed += ControllerTriggerPressed;
        ControllerEvents.ControllerTriggerUp += ControllerTriggerUp;

        text = GetComponent<TextMesh>();
        text.text = cloudName;

    }

    // Update is called once per frame
    void Update()
    {
        //print(triggerIsPressed);
        PointCloudActivator();
    }


    void OnTriggerEnter(Collider col)
    {
        //Quick scaling of object   
        Vector3 targetScale = originalScale + new Vector3(0.1f, 0.1f, 0.1f);
        if(col.gameObject.tag == "GameController")
        {
            transform.localScale = targetScale;
        }

            //Pass through Objects with trigger Pressed
        if (col.gameObject.tag == "GameController" && triggerIsPressed)
        {
            if (cloudIsActive)
            {
                print(pointCloud.name + " was Pressed");
                cloudIsActive = false;
            } else if (!cloudIsActive)
            {
                print(pointCloud.name + " was Pressed");
                cloudIsActive = true;
            }
        }

    }


    void OnTriggerExit()
    {
        transform.localScale = originalScale;
    }


    //Controller events
    public void ControllerTriggerPressed()
    {
        triggerIsPressed = true;
        //print("triggerPressed");
    }

    public void ControllerTriggerUp()
    {
        triggerIsPressed = false;
    }

    //Watch pointCloud state
    void PointCloudActivator()
    {
        if (cloudIsActive)
        {
            pointCloud.SetActive(true);
            text.color = activeColor;
        }
        if (!cloudIsActive)
        {
            pointCloud.SetActive(false);
            text.color = inactiveColor;
        }
    }
}
