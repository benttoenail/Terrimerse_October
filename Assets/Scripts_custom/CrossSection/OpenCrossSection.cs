using UnityEngine;
using System.Collections;
using System;

public class OpenCrossSection : MonoBehaviour {
    VRMenuButton myButton;
    
    public static event Action CrossSectionToggled;

    // Use this for initialization
    void Start () {

        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += HandleClick;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void HandleClick(VRMenuEventData e)
    {
        if(CrossSectionToggled != null)
        {
            CrossSectionToggled();
        }
    }
}
