using UnityEngine;
using System.Collections;

public class UIControls : MonoBehaviour {

    TextMesh text;
    Color colorHover;
    Color white;

    bool triggerDown;

    public bool intersected = false;

    ScaleDataSet scaleData;
    public GameObject dataSetOne;

	// Use this for initialization
	void Start () {

        text = GetComponent<TextMesh>();

        //dataSetOne = GameObject.Find("[CameraRig]");
        scaleData = (ScaleDataSet) dataSetOne.GetComponent(typeof(ScaleDataSet));

        colorHover = new Color(255, 0, 0);
        white = new Color(255, 255, 255);
        text.color = white;


    }
	
	// Update is called once per frame
	void Update () {

        triggerDown = GameObject.Find("Controller (right)").GetComponent<ViveControls>().triggerDown;

        if (intersected == true)
        {
            text.color = colorHover;

            if (triggerDown && this.gameObject.name == "size-1")
            {
                scaleData.SetScaleOne();
            }
            if (triggerDown && this.gameObject.name == "size-2")
            {
                scaleData.SetScaleTwo();
            }
            if (triggerDown && this.gameObject.name == "size-3")
            {
                scaleData.SetScaleThree();
            }
        }
        else
        {
            text.color = white;
        }

	}

    void OnTriggerEnter(Collider col)
    {
       // Debug.Log(this.gameObject.name);
        intersected = true;
        
    }

    void OnTriggerExit(Collider col)
    {
        intersected = false;

    }

}
