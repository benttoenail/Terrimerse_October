using UnityEngine;
using System.Collections;

public class OpenPCList : MonoBehaviour {
    VRMenuButton myButton;
    public GameObject PCListHandler;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += OpenList;

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
