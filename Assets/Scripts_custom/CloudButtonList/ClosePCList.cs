using UnityEngine;
using System.Collections;

public class ClosePCList : MonoBehaviour {
    public VRMenuButton myButton;
    [SerializeField] GameObject PCListHandler;

    // Use this for initialization
    void Start()
    {
        myButton = GetComponent<VRMenuButton>();
        myButton.OnClick += CloseList;

       // PCListHandler.SetActive(false);
    }

    void CloseList(VRMenuEventData e)
    {
        //PCListHandler.SetActive(false);
        Destroy(PCListHandler);
    }

}
