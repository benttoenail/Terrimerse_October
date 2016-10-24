using UnityEngine;
using System.Collections;

public class OpenPCList : MonoBehaviour {
    VRMenuButton myButton;
    GameObject PCListHandler;
    public GameObject PCListHandlerPrefab;

    GameObject player;
    GameObject playerRig;

    public Vector3 optimalPos;
    Vector3 playerPos;

    // Use this for initialization
    void Start () {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += OpenList;

        //PCListHandler = GameObject.FindGameObjectWithTag("PCList");
        player = GameObject.FindGameObjectWithTag("MainCamera");//Get player's headPosition
        playerRig = GameObject.FindGameObjectWithTag("Player");

        //PCListHandler.SetActive(false);
    }

    void OpenList(VRMenuEventData e)
    {
        //Get player position and move PCList to that position when opened
        //THIS IS A POOR MAN'S SOLUTION - DOES NOT TAKE ROTATION INTO ACCOUNT
        playerPos = player.transform.position;
        optimalPos = playerPos / playerRig.transform.localScale.x + new Vector3(0, 0, 1.7f);


        if (PCListHandler == null)
        {
            PCListHandler = Instantiate(PCListHandlerPrefab, optimalPos, Quaternion.identity) as GameObject;
           // PCListHandler.GetComponent<CloudButtonHandler>().InitCLoudList();
            PCListHandler.transform.SetParent(playerRig.transform, false);
        }else
        {
            Destroy(PCListHandler);
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
