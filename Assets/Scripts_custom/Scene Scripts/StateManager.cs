using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    public GameObject leftController;
    private ToolTracker leftToolTracker;

    public GameObject rightController;
    private ToolTracker rightToolTracker;

    public Material skyBox;



    // Use this for initialization
    void Start () {

        ToolTracker.ToolWasUpdated += LogTool;

        leftToolTracker = leftController.GetComponentInChildren<ToolTracker>();
        rightToolTracker = leftController.GetComponentInChildren<ToolTracker>();
    }
	

    void LogTool(GameObject _tool, GameObject _controller)
    {
        Debug.Log("Tool was updated!  " + _tool.name + " : " + _controller.name);
    }
}
