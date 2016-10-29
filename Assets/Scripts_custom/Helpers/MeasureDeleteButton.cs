using UnityEngine;
using System.Collections;

public class MeasureDeleteButton : MonoBehaviour {
	VRMenuButton myButton;
    public GameObject drawComponent;

	//public GameObject myPrefab; //Prefab Gameobject with toolScript attached

	// Use this for initialization
	void Start () {
		myButton = GetComponent<VRMenuButton> ();

		myButton.OnClick += DoStuff;
        myButton.OnIn += InTool;
        myButton.OnOut += OutTool;
	}

	void DoStuff(VRMenuEventData e) {

        GameObject measurementParent = transform.parent.gameObject;
        Destroy(measurementParent);
  
    }


    //Set "Measurement object" to delete if this object is Sphere_02
    public void SetDrawComponent(GameObject obj)
    {
        if (drawComponent == null)
        {
            drawComponent = obj;
        }
    }


    void InTool(VRMenuEventData e)
    {
        //Tell draw tool not to draw!
        //print("Measure button event FIRED!!");
        if(drawComponent != null)
        {
            drawComponent.GetComponent<DrawMeasurement>().IsInSphere();
        }
        
    }

    void OutTool(VRMenuEventData e)
    {
        //Tell tool to draw!!
        if(drawComponent != null)
        {
            drawComponent.GetComponent<DrawMeasurement>().IsOutSphere();
        }
        
    }
    

  
}