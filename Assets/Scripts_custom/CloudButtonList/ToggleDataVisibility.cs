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

        if(gameObject.name == "Button_Toggle_PC")
        {
            DataSet = GameObject.FindGameObjectWithTag("PointClouds");
        }
        else if (gameObject.name == "Button_Toggle_Mesh")
        {
            DataSet = GameObject.FindGameObjectWithTag("MeshClouds");
        }


        if(DataSet == null)
        {
            print(gameObject.name + "'s dataSet is empty!");
        }
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
