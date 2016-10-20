using UnityEngine;
using System.Collections;

public class BeltControl : MonoBehaviour {

	private int count;
    public float angle; //Radial distance between buttons
    public float space = 1.0f; //Distance from user
    public float angularSpace;

	//public GameObject beltIcon;
    public Material beltMat;

    GameObject[] nodes; // setting the angle for the icon objects
    Transform[] nodeIcon; // objects to be moved
	Quaternion q;
    public GameObject[] beltIcon = new GameObject[0];

    bool beltIsFixed;
    bool beltButtonCollide;
    bool beltIsOpen = false;

	// Use this for initialization
	void Start () {

        angle = 0;

        count = beltIcon.Length;

        nodes = new GameObject[count];
        nodeIcon = new Transform[count];
		GameObject temp = new GameObject();

        //Rotate and instantiate Object
		for(int i = 0; i < count; i++){
            
            //Set angle - instatiate prefab - create null rotation - parent icon to rotAngle - push into nodes array
			q.eulerAngles = new Vector3(0, i * angle / count, 0);                  
            if(beltIcon == null)
            {
                Debug.LogError("You forgot to add a tool to the belt!");
            }
            else
            {
                temp = Instantiate(beltIcon[i], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            }                                           
            
            GameObject rotAngle = new GameObject();
            temp.transform.SetParent(rotAngle.transform);
            rotAngle.transform.rotation = q;
            nodes[i] = rotAngle;
            nodeIcon[i] = nodes[i].transform.GetChild(0);
           
        }

	}

	// Update is called once per frame  
	void Update () {

        beltIsFixed = gameObject.GetComponent<HoverBelt>().beltPoisitionFixed; //Has belt been opened?
        beltButtonCollide = gameObject.GetComponent<HoverBelt>().controllerButtonCollide;

        //Attach to the HoverBelt_02 Object
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].transform.position = gameObject.transform.position;
            nodes[i].transform.eulerAngles = new Vector3(0, i * angle / count + transform.eulerAngles.y, 0);
        }

        if (beltIsFixed == true)
        {   
            StartCoroutine(OpenBelt());
        }
        if(beltIsFixed == false)
        {
            StartCoroutine(CloseBelt());
        }
    }


    //OPENING / CLOSING BELT ANIMATIONS\\
    IEnumerator OpenBelt()
    {
        StartCoroutine(FadeTo(1.0f, 1.0f));
        if (beltIsFixed && !beltIsOpen)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                iTween.MoveBy(nodeIcon[i].gameObject, iTween.Hash("x", space, "time", 1.0f));
                iTween.RotateBy(nodeIcon[i].gameObject, iTween.Hash("z", -0.1f, "delay", 1.0f));
               
            }
            beltIsOpen = true;

            yield return new WaitForSeconds(0.25f);
            iTween.ValueTo(gameObject, iTween.Hash("from", angle, "to", angularSpace, "time", 1.0f, "onupdate", "TweenBeltAngle", "easetype", iTween.EaseType.easeInOutQuad));
        }

    }


    IEnumerator CloseBelt()
    {
        StartCoroutine(FadeTo(0.0f, 0.1f));
        if (!beltIsFixed && beltIsOpen)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                iTween.RotateBy(nodeIcon[i].gameObject, iTween.Hash("z", 0.1f, "delay", 0.0f));
            }

            iTween.ValueTo(gameObject, iTween.Hash("from", angle, "to", 0, "time", 0.5f, "onupdate", "TweenBeltAngle", "easetype", iTween.EaseType.easeInOutQuad));
            beltIsOpen = false;
            yield return new WaitForSeconds(0.35f);

            for (int i = 0; i < nodes.Length; i++)
            {  
                iTween.MoveBy(nodeIcon[i].gameObject, iTween.Hash("x", -space, "time", 0.5f, "easetype", iTween.EaseType.easeInQuad));
            }
            
        }
    }
   
    //For tweening the angle float value
    void TweenBeltAngle(float _angle)
    {
        angle = _angle;
    }


    //Fade Alpha Channel on belt icons
    IEnumerator FadeTo(float aValue, float aTime)
    {
        if (beltMat != null)
        {
            float alpha = beltMat.color.a;
            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
                beltMat.color = newColor;
                yield return null;
            }
        }
    }

}
