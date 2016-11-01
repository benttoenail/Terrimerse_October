using UnityEngine;
using System.Collections;

public class ToolTracker : MonoBehaviour {

    public GameObject currentTool;
    public GameObject clonedTool;

    public GameObject interactor;

    Mesh mesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    Transform controller;
    GameObject controllerObj;


    public void UpdateCurrentTool(GameObject _currentTool, Mesh _mesh)
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
        ReplaceMesh(_mesh);
        
    }


	// Use this for initialization
	void Start () {
        controller = transform.parent;
        controllerObj = controller.gameObject;

        mesh = interactor.GetComponent<MeshFilter>().mesh;
        meshCollider = interactor.GetComponent<MeshCollider>();
    }
	
	// Update is called once per frame
	void Update () {

	}

    void ReplaceMesh(Mesh _mesh)
    {
        meshFilter = interactor.GetComponent<MeshFilter>();
        meshCollider = interactor.GetComponent<MeshCollider>();

        meshFilter.mesh = _mesh;
        meshCollider.sharedMesh = _mesh;
    }

}
