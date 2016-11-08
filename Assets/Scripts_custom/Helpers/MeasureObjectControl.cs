using UnityEngine;
using System.Collections;

public class MeasureObjectControl : MonoBehaviour {
    public GameObject drawComponent;

	GameObject dataSet;

    GameObject player;

	// Use this for initialization
	void Start () {

		dataSet = GameObject.FindGameObjectWithTag ("DataSet");

        player = GameObject.FindGameObjectWithTag("Player");

		//myButton = GetComponent<VRMenuItem> ();

		//myButton.OnClick += DoStuff;

        //print(player.transform.localScale);
	}

	//void DoStuff(VRMenuEventData e) {

 //       GameObject measurementParent = transform.parent.gameObject;
 //       Destroy(measurementParent);
  
 //   }
    
    void OnTriggerEnter(Collider col)
    {
        SteamVR_TrackedObject to = col.GetComponentInParent<SteamVR_TrackedObject>();
        if (to != null)
        {
            DrawMeasurement dm = to.GetComponentInChildren<DrawMeasurement>();
            if (dm != null)
            {
                dm.NotifySphereEnter(this);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        SteamVR_TrackedObject to = col.GetComponentInParent<SteamVR_TrackedObject>();
        if (to != null)
        {
            DrawMeasurement dm = to.GetComponentInChildren<DrawMeasurement>();
            if (dm != null)
            {
                dm.NotifySphereExit(this);
            }
        }
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

		Vector3 dataSetScale = dataSet.transform.localScale;

        transform.localScale =10 * new Vector3(1 / dataSetScale.x, 1 / dataSetScale.y, 1 / dataSetScale.z);
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