using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;


public class CloudBlock : MonoBehaviour {

    VRMenuCheckbox checkbox;

    public GameObject text;

    public string cloudName;
    GameObject pointCloud;
    GameObject meshCloud;

    public void GetCloudData(string n, GameObject pc, GameObject mc, bool isCloudActive)
	{
		checkbox = GetComponent<VRMenuCheckbox> ();
        cloudName = n;
        pointCloud = pc;
        meshCloud = mc;
		StartCoroutine (UpdateVis (isCloudActive));
    }


    IEnumerator UpdateVis(bool val) {
		yield return new WaitForEndOfFrame ();
		checkbox.state = val;
	}

    //Call from StateManager
    public void UpdateVisMethod(bool val)
    {
        StartCoroutine(UpdateVis(val));
    }

	// Use this for initialization
	void Start () {

		checkbox = GetComponent<VRMenuCheckbox> ();

		checkbox.OnStateChange += HandleStateChange;

        text.GetComponent<Text>().text = cloudName;

    }

    public bool currentState = false;

    public void HandleStateChange(bool visibleState)
    {
		pointCloud.SetActive (visibleState);
		meshCloud.SetActive (visibleState);
        currentState = visibleState;
    }

}
