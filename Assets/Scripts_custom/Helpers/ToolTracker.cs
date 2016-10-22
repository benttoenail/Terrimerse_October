using UnityEngine;
using System.Collections;

public class ToolTracker : MonoBehaviour {

    public GameObject currentTool;
    public GameObject clonedTool;

    Transform controller;
    GameObject controllerObj;


    public void UpdateCurrentTool(GameObject _currentTool)
    {
        if(currentTool != null)
        {
            //delete Current Tool
            GameObject temp = currentTool;
            Object.Destroy(temp);
        }

        //Attach tool to controller
        GameObject anInstance = (GameObject)Instantiate(_currentTool);
        anInstance.transform.SetParent(controllerObj.transform, false);
        currentTool = anInstance;
        
    }


	// Use this for initialization
	void Start () {
        controller = transform.parent;
        controllerObj = controller.gameObject;

    }
	
	// Update is called once per frame
	void Update () {

	}
}
