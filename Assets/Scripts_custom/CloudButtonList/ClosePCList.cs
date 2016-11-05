using UnityEngine;
using System.Collections;

public class ClosePCList : MonoBehaviour {
    public VRMenuButton myButton;
    [SerializeField] GameObject PCListHandler;

    public delegate void CLosedPCList();
    public static event CLosedPCList ListIsCLosed;

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

        //Send event to StateManager
        ListIsCLosed();
    }

}
