using UnityEngine;
using System.Collections;

public class ToggleDataVisibility : MonoBehaviour {

    VRMenuButton myButton;
    [SerializeField]
    GameObject DataSet;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += ToggleViz;
	}
	

    void ToggleViz(VRMenuEventData e)
    {
        if (DataSet.activeSelf)
        {
            DataSet.SetActive(false);
        }
        else
        {
            DataSet.SetActive(true);
        }
    }

}
