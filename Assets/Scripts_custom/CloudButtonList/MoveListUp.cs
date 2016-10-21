using UnityEngine;
using System.Collections;

public class MoveListUp : MonoBehaviour {
    
    bool triggerIsPressed;

    Vector3 originalScale;

    [SerializeField]
    GameObject PCListTransform;

    Vector3 listOriginalPos;

    float moveSpeed;
    float transformDelta;

    bool listIsMoving;
    bool controllerIntersecting;

    // Use this for initialization
    void Start () {

        ControllerEvents.ControllerTriggerPressed += ControllerTriggerPressed;
        ControllerEvents.ControllerTriggerUp += ControllerTriggerUp;

        originalScale = transform.localScale;

        PCListTransform = GameObject.Find("PCListTransform");
        listOriginalPos = PCListTransform.transform.position;

        if(PCListTransform == null)
        {
            Debug.LogError("PCListTransform cannot be found");
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (triggerIsPressed && controllerIntersecting )
        {
            if (gameObject.name== "UpArrow")
            {
                transformDelta += 5.0f;
            }
            if (gameObject.name == "DownArrow")
            {
                transformDelta -= 5.0f;
            }

            Vector3 temp = new Vector3(listOriginalPos.x, listOriginalPos.y + transformDelta, listOriginalPos.z);
            PCListTransform.transform.position = temp;

        } else if(!triggerIsPressed)
        { 
            PCListTransform.transform.position = PCListTransform.transform.position;
        }

    }

    void OnTriggerEnter(Collider col)
    {
        //Simple scaling of object
        Vector3 targetScale = originalScale + new Vector3(10f, 10f, 10f);
        transform.localScale = targetScale;

        controllerIntersecting = true;
        listOriginalPos = PCListTransform.transform.position;

    }

    void OnTriggerExit()
    {
        transform.localScale = originalScale;
        listIsMoving = false;
        controllerIntersecting = false;
        transformDelta = 0;
    }


    //Controller events
    public void ControllerTriggerPressed()
    {
        triggerIsPressed = true;
        print("triggerPressed from MoveScript");
    }

    public void ControllerTriggerUp()
    {
        triggerIsPressed = false;
        print("Trigger is up from move script");
    }
}
