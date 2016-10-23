using UnityEngine;
using System.Collections;

public class OpenPCList : MonoBehaviour {
    VRMenuButton myButton;
    GameObject PCListHandler;

    GameObject player;
    public Vector3 optimalPos;
    Vector3 playerPos;

    // Use this for initialization
    void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += OpenList;

        PCListHandler = GameObject.FindGameObjectWithTag("PCList");
        player = GameObject.FindGameObjectWithTag("MainCamera");//Get player's headPosition

        PCListHandler.SetActive(false);
    }

    void OpenList(VRMenuEventData e)
    {
        //Get player position and move PCList to that position when opened
        //THIS IS A POOR MAN'S SOLUTION - DOES NOT TAKE ROTATION INTO ACCOUNT
        playerPos = player.transform.position;
        optimalPos = new Vector3(0, 0, 170);
        PCListHandler.transform.position = playerPos + optimalPos;

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
	    
        /*
        if(PCListHandler != null)
        {
            print("Moving the list");
            PCListHandler.transform.position = playerPos + optimalPos;
        }*/
	}
}
