using UnityEngine;
using System.Collections;

public class OpenPCList : MonoBehaviour {
    VRMenuButton myButton;
    GameObject PCListHandler;
    public GameObject PCListHandlerPrefab;

    GameObject player;
    GameObject playerRig;

    //public Vector3 optimalPos;
    Vector3 playerPos;

    //For Debugging
    GameObject debugShape;

    // Use this for initialization
    void Start () {

        debugShape = GameObject.FindGameObjectWithTag("MeasureSphere");

        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += OpenList;

        //PCListHandler = GameObject.FindGameObjectWithTag("PCList");
        //player = GameObject.FindGameObjectWithTag("MainCamera");//Get player's headPosition
        //playerRig = GameObject.FindGameObjectWithTag("Player");

        //PCListHandler.SetActive(false);
    }

    void OpenList(VRMenuEventData e)
    {
        //Get player position and move PCList to that position when opened

        player = GameObject.FindGameObjectWithTag("MainCamera");//Get player's headPosition
        playerRig = GameObject.FindGameObjectWithTag("Player");

        float fwd = 2.5f;
        float divideScale = playerRig.transform.localScale.x;
        playerPos = player.transform.position; // playerRig.transform.localScale.x;
        
        Vector3 spawnPos = playerPos / divideScale;//Vector3.Scale(player.transform.localPosition, playerRig.transform.localScale);
        Vector3 optimalPos = (spawnPos + player.transform.forward * fwd) - (playerRig.transform.position / divideScale);

        //Quaternion rotationPos = Quaternion.LookRotation(spawnPos); //spawnPos - (playerRig.transform.position / divideScale));
        Quaternion rotationPos = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
        Quaternion finalRotation = new Quaternion(0, rotationPos.y, 0, 0);

        if (PCListHandler == null)
        {
            PCListHandler = Instantiate(PCListHandlerPrefab, optimalPos, rotationPos) as GameObject; // For Debugging
            PCListHandler.transform.SetParent(playerRig.transform, false);
            //PCListHandler.transform.LookAt(player.transform.position);

            // PCListHandler.GetComponent<CloudButtonHandler>().InitCLoudList();
            // GameObject thisShape = Instantiate(debugShape, optimalPos, Quaternion.identity) as GameObject;
            // thisShape.transform.SetParent(playerRig.transform, false);
        }
        else
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
