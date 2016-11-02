using UnityEngine;
using System.Collections;

public class MeasureObjectControl : MonoBehaviour {
	VRMenuButton myButton;
    public GameObject drawComponent;

    GameObject player;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

		myButton = GetComponent<VRMenuButton> ();

		myButton.OnClick += DoStuff;
        myButton.OnIn += InTool;
        myButton.OnOut += OutTool;

        //print(player.transform.localScale);
	}

	void DoStuff(VRMenuEventData e) {

        GameObject measurementParent = transform.parent.gameObject;
        Destroy(measurementParent);
  
    }

    void Update()
    {
        if (ScaleDataAndObjects.isScaling)
        {
            return;
        }
        else
        {
            ScaleMeasureObjects();
        }
        
    }

    void ScaleMeasureObjects()
    {
        float max = 350.0f;
        float min = 50.0f;
        Vector3 playerScale = player.transform.localScale;

        if(playerScale.x > max || playerScale.x < min)
        {
            return;
        }
        else
        {
            transform.localScale = player.transform.localScale / 10;
        }
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