using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    public GameObject leftController;
    private ToolTracker leftToolTracker;

    public GameObject rightController;
    private ToolTracker rightToolTracker;

    public Material skyBox;
    public float topHorizon = 20;
    public float bottomHorizon = 20;
    [SerializeField]
    AnimationCurve BKGCurve;

    // Use this for initialization
    void Start () {

        //Set Starting Horizon colors
        skyBox.SetFloat("_Exponent1", topHorizon);
        skyBox.SetFloat("_Exponent2", bottomHorizon);

        ToolTracker.ToolWasUpdated += LogTool;
        OpenPCList.PCListOpened += PClistIsOpen;
        OpenPCList.PCListClosed += PCListIsClosed;
        ClosePCList.ListIsCLosed += PCListIsClosed;

        leftToolTracker = leftController.GetComponentInChildren<ToolTracker>();
        rightToolTracker = leftController.GetComponentInChildren<ToolTracker>();
    }
	

    void LogTool(GameObject _tool, GameObject _controller)
    {
        //Get information on the Current tool
        //Turn off tool when in list view?
        //Will have to replace the current tool with null - switch back when list is back off
        //Debug.Log("Tool was updated!  " + _tool.name + " : " + _controller.name);
    }

    void PClistIsOpen()
    {
        StartCoroutine(BKGColorChange(0.5f, topHorizon, 0));
    }

    void PCListIsClosed()
    {
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

            skyBox.SetFloat("_Exponent1", Mathf.Lerp(startColor, endColor, BKGCurve.Evaluate(elapsedTime)));
            skyBox.SetFloat("_Exponent2", Mathf.Lerp(startColor, endColor, BKGCurve.Evaluate(elapsedTime)));

            yield return new WaitForEndOfFrame();
        }
    }
}
