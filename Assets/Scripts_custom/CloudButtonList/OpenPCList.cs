using UnityEngine;
using System.Collections;

public class OpenPCList : MonoBehaviour {
    VRMenuButton myButton;
    GameObject PCListHandler;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += OpenList;
        PCListHandler = GameObject.FindGameObjectWithTag("PCList");

        PCListHandler.SetActive(false);
    }

    void OpenList(VRMenuEventData e)
    {
        if (!PCListHandler.activeSelf)
        {
            PCListHandler.SetActive(true);
        }else
        {
            PCListHandler.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update () {
	
	}
}
