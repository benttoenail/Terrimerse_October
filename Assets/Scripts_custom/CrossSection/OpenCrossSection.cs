using UnityEngine;
using System.Collections;
using System;

public class OpenCrossSection : MonoBehaviour {
    VRMenuButton myButton;
	HoverBeltItems hoverBelt;

    public static event Action CrossSectionToggled;



    // Use this for initialization
    void Start () {

        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += HandleClick;
		hoverBelt = GetComponentInParent<HoverBeltItems> ();
	}

    void HandleClick(VRMenuEventData e)
    {
        if(CrossSectionToggled != null)
        {
            CrossSectionToggled();
        }
		hoverBelt.DoClose ();
    }
}
