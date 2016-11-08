using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {

    public GameObject leftController;
    private ToolTracker leftToolTracker;

    public GameObject rightController;
    private ToolTracker rightToolTracker;

    public Material skyBox;
    public float topHorizon = 20;
    public float bottomHorizon = 20;
    [SerializeField]
    AnimationCurve animCurve;

    public GameObject crossSection;
    public GameObject DataSet;
    public bool crossSectionOpened;

    private Vector3 dataSetOriPos;
    private Vector3 dataSetOriScale;
    private Quaternion dataSetOriRot;

    private GameObject meshClouds;
    private GameObject pointClouds;

    private GameObject MainDataSet;
    // Use this for initialization
    void Start () {

        //Find the Main DataSet Group
        MainDataSet = GameObject.FindGameObjectWithTag("DataSet");

        //Turn off CrossSection on Start
        crossSection.SetActive(false);

        //Set Starting Horizon colors
        skyBox.SetFloat("_Exponent1", topHorizon);
        skyBox.SetFloat("_Exponent2", bottomHorizon);

        //PcList Events
        OpenPCList.PCListOpened += PClistIsOpen;
        OpenPCList.PCListClosed += PCListIsClosed;
        ClosePCList.ListIsCLosed += PCListIsClosed;

        //CrossSection Events
        ToolTracker.ToolWasUpdated += LogTool;
        OpenCrossSection.CrossSectionToggled += ToggleCrossSection;

        //Scene Reset Event
        ResetScene.SceneReset += DoSceneReset;

        meshClouds = GameObject.FindGameObjectWithTag("MeshClouds");
        pointClouds = GameObject.FindGameObjectWithTag("PointClouds");

        //Get Tools for each controller
        leftToolTracker = leftController.GetComponentInChildren<ToolTracker>();
        rightToolTracker = rightController.GetComponentInChildren<ToolTracker>();

        //Store the Starting position and rotation of DataSet
        dataSetOriPos = MainDataSet.transform.position;
        dataSetOriRot = MainDataSet.transform.rotation;
        dataSetOriScale = MainDataSet.transform.localScale;
    }
	

    // - - - - THE CROSSSECTION - - - - \\
    void LogTool(GameObject _tool, GameObject _controller)
    {
        //Debug.Log("Tool was updated!  " + _tool.name + " : " + _controller.name);
    }

    void ToggleCrossSection()
    {
        crossSection.SetActive(!crossSection.activeSelf);
        DataSet.SetActive(!DataSet.activeSelf);
    }


    // - - - - SCENE RESET - - - - \\
    void DoSceneReset()
    {
        Debug.Log("Resetting the Scene!");


        //Reset Position of DataSet
        StartCoroutine(ReturnToOriginalPosition(0.4f, MainDataSet));

        //Cross Section Planes return to Original Position
        MoveSlice[] moveSlice = crossSection.GetComponentsInChildren<MoveSlice>();
        foreach (MoveSlice ms in moveSlice)
        {
            ms.GetComponent<MoveSlice>().MoveToOriginalPosition();
        }


        //Set all clouds back to active
        //Need to access PCListTransform - but will only be available when PCList is opened
        Transform pcListTransform = PCList.transform.FindChild("PCListTransform");
        
        foreach(Transform c in pcListTransform)
        {
            bool currentState = c.GetComponent<CloudBlock>().currentState;
            if (currentState)
            {
                break;
            }else
            {
                c.GetComponent<CloudBlock>().HandleStateChange(true);
                c.GetComponent<CloudBlock>().UpdateVisMethod(true);
            }
        }

        //Delete All measurements
        //List<GameObject> measurements = new List<GameObject>();
        GameObject[] measurements;
        measurements = GameObject.FindGameObjectsWithTag("MeasureTool");
        
        for(int i =0; i < measurements.Length; i++)
        {
            Destroy(measurements[i]);
        }

    }
    
    //Animate the 
    IEnumerator ReturnToOriginalPosition(float time, GameObject ds)
    {
        float elapsedTime = 0;
        Vector3 dsPos = ds.transform.position;
        Vector3 dsScale = ds.transform.localScale;
        Quaternion dsRot = ds.transform.rotation;
        float startTime = Time.time;

        while(elapsedTime < 1)
        {
            elapsedTime = Time.time - startTime;
            ds.transform.position = Vector3.Lerp(dsPos, dataSetOriPos, animCurve.Evaluate(elapsedTime));
            ds.transform.localScale = Vector3.Lerp(dsScale, dataSetOriScale, animCurve.Evaluate(elapsedTime));
            ds.transform.rotation = Quaternion.Slerp(dsRot, dataSetOriRot, animCurve.Evaluate(elapsedTime));

            yield return new WaitForEndOfFrame();
        }
    }


    // - - - - THE PCLIST - - - - \\
    GameObject PCList;

    void PClistIsOpen()
    {
        PCList = GameObject.FindGameObjectWithTag("PCList");
        StartCoroutine(BKGColorChange(0.5f, topHorizon, 0));
        print(PCList.name);
    }

    void PCListIsClosed()
    {
        PCList = null;
        StartCoroutine(BKGColorChange(0.5f, 0, topHorizon));
    }

    //Animation Coroutine for Changing BKG Color
    IEnumerator BKGColorChange(float time, float startColor, float endColor)
    {
        float elapsedTime = 0;
        float zeroTop = topHorizon;
        float zeroBottom = bottomHorizon;
        float startTime = Time.time;

        while (elapsedTime < 1)
        {
            elapsedTime = Time.time - startTime;

            skyBox.SetFloat("_Exponent1", Mathf.Lerp(startColor, endColor, animCurve.Evaluate(elapsedTime)));
            skyBox.SetFloat("_Exponent2", Mathf.Lerp(startColor, endColor, animCurve.Evaluate(elapsedTime)));

            yield return new WaitForEndOfFrame();
        }
    }
}
