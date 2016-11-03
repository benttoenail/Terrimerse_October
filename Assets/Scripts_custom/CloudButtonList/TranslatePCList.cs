using UnityEngine;
using System.Collections;

public class TranslatePCList : MonoBehaviour {

    VRMenuButton myButton;

    GameObject PCList;

    bool listIsMoving;
    public float speed;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<VRMenuButton>();

        myButton.OnClick += IncreaseSpeed;
        myButton.OnIn += TranslateList;
        myButton.OnOut += StopList;

        PCList = GameObject.Find("PCListTransform");

        
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 tempMove = new Vector3(0, 2.5f, 0);
        if (listIsMoving)
        {
            if(this.gameObject.name == "Button_up")
            {
                PCList.transform.position += tempMove;// * speed
            }else
            {
                PCList.transform.position -= tempMove;// * speed
            }
            
        }
       
	}

    void IncreaseSpeed(VRMenuEventData e)
    {
        speed = 5;
    }

    void TranslateList(VRMenuEventData e)
    {
        listIsMoving = true;
    }

    void StopList(VRMenuEventData e)
    {
        listIsMoving = false;
    }
}
