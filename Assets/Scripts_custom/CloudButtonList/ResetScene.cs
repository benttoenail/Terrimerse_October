using UnityEngine;
using System.Collections;
using System;

public class ResetScene : MonoBehaviour {
    VRMenuButton myButton;

    public static event Action SceneReset;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += HandleClick;
	
	}
	
    void HandleClick(VRMenuEventData e)
    {
        if(SceneReset != null)
        {
            SceneReset();
        }
        
    }

    // Update is called once per frame
    void Update () {
	
	}
}
