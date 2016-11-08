using UnityEngine;
using System.Collections;

public class ToolTracker : MonoBehaviour {

    public GameObject currentTool;
    public GameObject clonedTool;

    public GameObject interactor;

    Mesh defaultMesh;
    Mesh mesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    Transform controller;
    GameObject controllerObj;

    public delegate void ToolUpdated(GameObject _tool, GameObject _controller);
    public static event ToolUpdated ToolWasUpdated;


    // Use this for initialization
    void Start()
    {
        controller = transform.parent;
        controllerObj = controller.gameObject;

        mesh = interactor.GetComponent<MeshFilter>().mesh;
        meshCollider = interactor.GetComponent<MeshCollider>();
        defaultMesh = mesh;
    }


    public void UpdateCurrentTool(GameObject toolPrefab, Mesh _mesh)
    {
        if(currentTool != null)
        {
            //delete Current Tool
            GameObject temp = currentTool;
            Object.Destroy(temp);
        }

        //Attach tool to controller
        GameObject tool = (GameObject)Instantiate(toolPrefab);
        tool.transform.SetParent(controllerObj.transform, false);
        currentTool = tool;
        ReplaceMesh(_mesh);

        //Send Event to StateManager
        ToolWasUpdated(currentTool, controllerObj);

        interactor.GetComponent<ControllerMenuInteractor>().currentFunctionality = tool.GetComponent<ControllerFunctionality>();

    }

    void ReplaceMesh(Mesh _mesh)
    {
        meshFilter = interactor.GetComponent<MeshFilter>();
        meshCollider = interactor.GetComponent<MeshCollider>();

        meshFilter.mesh = _mesh;
        meshCollider.sharedMesh = _mesh;
    }

    //DeActivate or ReActivate when CrossSection is opened
    Mesh currentMesh;
    public void DeActivateTool()
    {
        if(currentTool != null)
        {
            currentMesh = meshFilter.mesh;
            currentTool.SetActive(false);
            meshFilter = interactor.GetComponent<MeshFilter>();
            meshFilter.mesh = defaultMesh;
        }

    }

    public void ActivateTool()
    {
        if(currentTool != null)
        {
            currentTool.SetActive(true);
            meshFilter = interactor.GetComponent<MeshFilter>();
            meshFilter.mesh = currentMesh;
        }
    }

}
