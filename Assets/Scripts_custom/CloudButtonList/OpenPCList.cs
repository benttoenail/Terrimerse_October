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

    //Send event to StateManager
    public delegate void PCListOpen();
    public static event PCListOpen PCListOpened;
    public static event PCListOpen PCListClosed;

    // Use this for initialization
    void Start () {

        debugShape = GameObject.FindGameObjectWithTag("MeasureSphere");

        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += OpenList;

    }

    void OpenList(VRMenuEventData e)
    {

        //Send event to StateManager
        //PCListOpened();

        //Get player position and move PCList to that position when opened

        player = GameObject.FindGameObjectWithTag("MainCamera");//Get player's headPosition
        playerRig = GameObject.FindGameObjectWithTag("Player");

        
        float divideScale = playerRig.transform.localScale.x;
        float fwd = 0.7f;
        playerPos = player.transform.position;
        
        //Calculate position based on parenting and scale of rig
        Vector3 spawnPos = playerPos / divideScale;
        Vector3 optimalPos = (spawnPos + player.transform.forward * fwd) - (playerRig.transform.position / divideScale);

        //Set Rotation for PCList 
        Quaternion rotationPos = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);

        if (PCListHandler == null)
        {
            PCListHandler = Instantiate(PCListHandlerPrefab, optimalPos, rotationPos) as GameObject; // For Debugging
            PCListHandler.transform.SetParent(playerRig.transform, false);

            PCListHandler.GetComponent<CloudButtonHandler>().InitDataSet(rotationPos);

            PCListOpened();
        }
        else
        {
            Destroy(PCListHandler);
            PCListClosed();
        }
        
    }

}
