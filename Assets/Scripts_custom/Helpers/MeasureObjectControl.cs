using UnityEngine;
using System.Collections;

public class MeasureObjectControl : MonoBehaviour {
	VRMenuItem myButton;
    public GameObject drawComponent;

	GameObject dataSet;

    GameObject player;

	// Use this for initialization
	void Start () {

		dataSet = GameObject.FindGameObjectWithTag ("DataSet");

        player = GameObject.FindGameObjectWithTag("Player");

		myButton = GetComponent<VRMenuItem> ();

		myButton.OnClick += DoStuff;

        //print(player.transform.localScale);
	}

	void DoStuff(VRMenuEventData e) {

        GameObject measurementParent = transform.parent.gameObject;
        Destroy(measurementParent);
  
    }

    void Update()
    {
		if (!ScaleDataAndObjects.isScaling) {
			ScaleMeasureObjects ();
		}
        
    }

    void ScaleMeasureObjects()
    {
        float max = 350.0f;
        float min = 50.0f;

		Vector3 dataSetScale = dataSet.transform.localScale / 0.1f;

		transform.localScale = dataSetScale;
    }


    //Set "Measurement object" to delete if this object is Sphere_02
    public void SetDrawComponent(GameObject obj)
    {
        if (drawComponent == null)
        {
            drawComponent = obj;
        }
    }
    

  
}