using UnityEngine;
using System.Collections;
using System;

public class SetAllState : MonoBehaviour {
    VRMenuButton myButton;
	public bool stateToSet;

	public GameObject PCListTransform;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += HandleClick;
	}
	
    void HandleClick(VRMenuEventData e)
    {
		foreach (CloudBlock cb in PCListTransform.GetComponentsInChildren<CloudBlock>()) {
			if (cb.currentState != stateToSet) {
				cb.GetComponent<VRMenuCheckbox> ().ForceToggle ();
			}
		}
    }
}
