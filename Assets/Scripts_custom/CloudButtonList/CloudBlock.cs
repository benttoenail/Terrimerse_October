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
    GameObject meshCloud;

    public void GetCloudData(string n, GameObject pc, GameObject mc, bool isCloudActive)
    {
        cloudName = n;
        pointCloud = pc;
        meshCloud = mc;
        vizSphere.SetActive(isCloudActive);
        print(isCloudActive);
    }

	// Use this for initialization
	void Start () {

        myButton = GetComponent<VRMenuButton>();
        myButton.OnPointerDown += ToggleViz;

        text.GetComponent<Text>().text = cloudName;
        
    }

    void ToggleViz(VRMenuEventData e)
    {
        print("clicked!");
        if (pointCloud.activeSelf)
        {
            pointCloud.SetActive(false);
            meshCloud.SetActive(false);

            print("Turning off PC");
            print(pointCloud.activeSelf);
            vizSphere.SetActive(false);
        } else
        {
            pointCloud.SetActive(true);
            meshCloud.SetActive(true);

            print("Turning On PC");
            print(pointCloud.activeSelf);
            vizSphere.SetActive(true);
        }
    }

}
