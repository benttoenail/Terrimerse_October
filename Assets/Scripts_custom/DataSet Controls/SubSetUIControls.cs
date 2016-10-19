using UnityEngine;
using System.Collections;

public class SubSetUIControls : MonoBehaviour {

	//Subset of data this control is controlling
	public GameObject dataSubSet;

	ToggleDataSet DataSet;

    TextMesh text;
    Color colorHover;
	Color activated;
    Color white;

    bool triggerDown;

    public bool intersected = false;

	// Use this for initialization
	void Awake () {

        text = GetComponent<TextMesh>();
		DataSet = GameObject.FindGameObjectWithTag ("DataSet").GetComponent<ToggleDataSet>();


        //dataSetOne = GameObject.Find("[CameraRig]");
        //scaleData = (ScaleDataSet) dataSetOne.GetComponent(typeof(ScaleDataSet));

        colorHover = new Color(255, 0, 0);
		activated = new Color (255, 255, 0);
        white = new Color(255, 255, 255);
        text.color = white;

		dataSubSet.gameObject.SetActive (false);	


    }
	
	// Update is called once per frame
	void Update () {

		triggerDown = GameObject.Find ("Controller (right)").GetComponent<ViveControls> ().triggerDown;

		if (intersected == true) {
			text.color = colorHover;

			//SUBSET ONE
			if (triggerDown && this.gameObject.name == "set-1") {
				DataSet.ShowSubSetOne ();
				text.color = activated;
			}

			//SUBSET TWO
			if (triggerDown && this.gameObject.name == "set-2") {
				DataSet.ShowSubSetTwo ();
				text.color = activated;
			}

			//SUBSET THREE
			if (triggerDown && this.gameObject.name == "set-3") {
				DataSet.ShowSubSetThree ();
				text.color = activated;
			}

			//SUBSET FOUR
			if (triggerDown && this.gameObject.name == "set-4") {
				DataSet.ShowSubSetFour ();
				text.color = activated;
			}

			//SUBSET FIVE
			if (triggerDown && this.gameObject.name == "set-5") {
				DataSet.ShowSubSetFive ();
				text.color = activated;
			}

	   } else {
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
