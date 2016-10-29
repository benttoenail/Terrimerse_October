using UnityEngine;
using System.Collections;

public class ToggleDataVisibility : MonoBehaviour {

    VRMenuButton myButton;
    [SerializeField]
    GameObject DataSet;
    Transform DataSetChild;

    Transform vizSphere;

	// Use this for initialization
	void Start () {

        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += ToggleViz;

        DataSet = GameObject.FindGameObjectWithTag("DataSet");

        //Find appropriate DataSet based on Object Name
        if(gameObject.name == "Button_Toggle_PC")
        {
            DataSetChild = DataSet.gameObject.transform.Find("DataSet 17 - 41/PointCloud_dataSet");
            vizSphere = gameObject.transform.Find("VizSphere");
        }
        else if (gameObject.name == "Button_Toggle_Mesh")
        {
            DataSetChild = DataSet.gameObject.transform.Find("DataSet 17 - 41/MeshedCloud_dataSet");
            vizSphere = gameObject.transform.Find("VizSphere");
        }

        if(DataSet == null)
        {
            print(gameObject.name + "'s dataSet is empty!");
        }

        //Turn off VizSphere if Cloud group is off upon instatiation
        if (!DataSetChild.gameObject.activeSelf)
        {
            vizSphere.gameObject.SetActive(false);
        }
    }
	

    void ToggleViz(VRMenuEventData e)
    {

        if (DataSetChild.gameObject.activeSelf)
        {
            DataSetChild.gameObject.SetActive(false);
            vizSphere.gameObject.SetActive(false);
        }
        else
        {
            DataSetChild.gameObject.SetActive(true);
            vizSphere.gameObject.SetActive(true);
        }
    }

}
