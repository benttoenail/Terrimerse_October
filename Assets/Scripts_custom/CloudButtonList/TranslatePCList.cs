using UnityEngine;
using System.Collections;

public class TranslatePCList : MonoBehaviour {

    VRMenuButton myButton;

    GameObject PCList;

    bool listIsMoving;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<VRMenuButton>();

        myButton.OnIn += TranslateList;
        myButton.OnOut += StopList;

        PCList = GameObject.Find("PCListTransform");

        
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 tempMove = new Vector3(0, 1, 0);

        print(listIsMoving);

        if (listIsMoving)
        {
            if(this.gameObject.name == "Button_up")
            {
                PCList.transform.position += tempMove;
            }else
            {
                PCList.transform.position -= tempMove;
            }
            
        }
       
	}

    void TranslateList(VRMenuEventData e)
    {
        print("Moving list!");
        listIsMoving = true;
    }

    void StopList(VRMenuEventData e)
    {
        print("Stoping list!");
        listIsMoving = false;
    }
}
