using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;


public class CloudBlock : MonoBehaviour {

    VRMenuButton myButton;

    public GameObject text;
    public GameObject vizSphere;
    public string cloudName;
    GameObject pointCloud;

    /* // For changing color of text -- May be uneccessary...  
    Color activeColor = new Color(50, 50, 200);//lightBlue
    Color inactiveColor = new Color(100, 100, 100);//grey
    */

    public void GetCloudData(string n, GameObject pc)
    {
        cloudName = n;
        pointCloud = pc;
    }

	// Use this for initialization
	void Start () {

        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += ToggleViz;

        text.GetComponent<Text>().text = cloudName;
    }

    void ToggleViz(VRMenuEventData e)
    {
        print("clicked!");
        if (pointCloud.activeSelf)
        {
            pointCloud.SetActive(false);
            print("Turning off PC");
            print(pointCloud.activeSelf);
            vizSphere.SetActive(false);
        }else
        {
            pointCloud.SetActive(true);
            print("Turning On PC");
            print(pointCloud.activeSelf);
            vizSphere.SetActive(true);
        }
    }

}
