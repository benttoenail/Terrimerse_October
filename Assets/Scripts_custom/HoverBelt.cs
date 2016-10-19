using UnityEngine;
using System.Collections;

public class HoverBelt : MonoBehaviour {

    public GameObject headSet;

    Vector3 beltPosition;
    Vector3 fixedPosition;
    Quaternion beltRotation;

    public float height;
    public float moveSpeed;
    public float beltAngle;

    public bool beltPoisitionFixed = false;
    public bool controllerCollide = false;
    public bool controllerButtonCollide = false;

    // Use this for initialization
    void Start()
    {
        //headSet = GameObject.Find("Camera (head)"); //Can't find the object by name -- dragging and dropping for now
        if(headSet == null)
        {
            Debug.Log("HEADSET NOT FOUND YOU WHORE!!");
        }

        //Subscribe to ControllerEvents script
        ControllerEvents.ControllerEntered += ControllerEntered;
        ControllerEvents.ControllerExited += ControllerExited;
        ControllerEvents.ControllerTriggerPressed += ControllerTriggerPressed;
        ControllerEvents.ControllerCollideAndTrigger += ControllerCollideAndTrigger;

        ControllerEvents.ControllerEnteredButton += ControllerEnteredButton;
        ControllerEvents.ControllerButtonPressed += ControllerButtonPressed;
        ControllerEvents.ControllerExitedButton += ControllerExitedButton;

    }

    // Update is called once per frame
    void Update()
    {

        if(headSet != null)
        {
            if (beltPoisitionFixed == false)
            {
                MoveWithHeadSet();
            }
        }
        
    }


    //When Belt is not locked in placeS
    void MoveWithHeadSet()
    {
        beltPosition = new Vector3(headSet.transform.position.x, headSet.transform.position.y + height, headSet.transform.position.z);
        gameObject.transform.position = Vector3.Lerp(transform.position, beltPosition, Time.deltaTime * moveSpeed);
        beltRotation = Quaternion.Euler(new Vector3(0, headSet.transform.eulerAngles.y - beltAngle, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, beltRotation, Time.deltaTime * moveSpeed);
    }


    //When trigger is clicked with collision, fix the belt position
    void FixBeltPosition()
    {
        Debug.Log("Fixing the belt position");
        beltPosition = new Vector3(headSet.transform.position.x, headSet.transform.position.y + height, headSet.transform.position.z);
        gameObject.transform.position = Vector3.Lerp(transform.position, beltPosition, Time.deltaTime * moveSpeed);
        beltRotation = Quaternion.Euler(new Vector3(0, headSet.transform.eulerAngles.y - beltAngle, 0));
        transform.rotation = Quaternion.Slerp(transform.rotation, beltRotation, Time.deltaTime * moveSpeed);

        beltPoisitionFixed = true;
    }


    //SUBCSCRIBED EVENTS
    public void ControllerEntered()
    {
       // Debug.Log("Controller has ENTERED the building!");
    }

    public void ControllerExited()
    {
       // Debug.Log("Controller has LEFT the building!");
    }

    public void ControllerTriggerPressed()
    {
      //  Debug.Log("Trigger Has been Pressed!!");
      if(beltPoisitionFixed == true)
        {
            beltPoisitionFixed = false;
        }

    }

    public void ControllerCollideAndTrigger()
    {
        if (beltPoisitionFixed == false)
        {
            FixBeltPosition();
        }
    }

    public void ControllerTriggerUp()
    {
        //Debug.Log("Trigger Released");
    }

    public void ControllerEnteredButton()
    {
        controllerButtonCollide = true;
        Debug.Log("Button event is fired!");
    }

    public void ControllerButtonPressed()
    {
        Debug.Log("A BeltButton was presseaed!!");
    }

    public void ControllerExitedButton()
    {
        controllerButtonCollide = false;
    }
}
