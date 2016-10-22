using UnityEngine;
using System.Collections;

public class HoverBeltItems : MonoBehaviour {
	public float space = 1.0f; // Radial distance from user
	public float angularRange = 150; // Total spread of items
	public float minSepAngle = 10; // Minimum angle between items in degrees
	public float maxSepAngle= 30; // Maximum angle between items in degrees
	public float pitchAngle = 30; // Angle of inclination
	public float openTime = 1.0f; // Duration of the opening animation in seconds
	public float closeTime = 0.5f; // Duration of the closing animation in seconds
	public float moveSpeed = 4.0f; // Speed at which the belt follows the player

	public float baseHeight = -0.7f;
	public float raiseHeight = 0.5f;


	//public GameObject beltIcon;
	public Material beltMat;


	public enum BeltState {
		Open,
		Closed
	}
	public BeltState state = BeltState.Closed;

    



	int numItems;
	GameObject[] items;
	GameObject[] nodes;

	public VRMenuButton beltOpenButton;
	ControllerMenuInteractor tempLeftInteractor;
	ControllerMenuInteractor tempRightInteractor;

	public void Configure (GameObject head, GameObject playerParent, GameObject left, GameObject right) { // Set player Parent to new object
		GetComponentInParent<HoverBeltMotion> ().Configure (head, playerParent);
		tempLeftInteractor = left.GetComponentInChildren<ControllerMenuInteractor>();
		tempRightInteractor = right.GetComponentInChildren<ControllerMenuInteractor>();;

		// Assign listeners
		beltOpenButton.OnClick += HandleBeltClick;
		tempLeftInteractor.OnEmptyClick += HandleOffClick;
		tempRightInteractor.OnEmptyClick += HandleOffClick;

		// Acquire menu items and reparent them to rotation nodes
		numItems = transform.childCount;
		Transform[] temp = new Transform[numItems];
		for(int i = 0; i < transform.childCount; i++) {
			temp [i] = transform.GetChild(i);
		}

		nodes = new GameObject[numItems];
		items = new GameObject[numItems];

		for(int i = 0; i < numItems; i++) {
			GameObject node = new GameObject ();
			node.name = "Rotation node (" + temp [i].name + ")";
			node.transform.SetParent (this.transform,false);
			temp[i].SetParent(node.transform,false);
			nodes [i] = node;
			items [i] = temp[i].gameObject;

			nodes [i].transform.rotation = Quaternion.Euler (0 ,0, 0);
			nodes [i].SetActive (false);
		}
	}

	void HandleBeltClick(VRMenuEventData e) {
		if (state == BeltState.Open) {
			DoClose ();
		} else {
			DoOpen ();
		}
	}

	void HandleOffClick(VRMenuEventData e) {
		if (state == BeltState.Open) {
			DoClose ();
		}
	}

	void DoOpen() {
		state = BeltState.Open;
		StartCoroutine (AnimateOpen ());
	}
	void DoClose() {
		state = BeltState.Closed;
		StartCoroutine (AnimateClose ());
	}

    //OPENING / CLOSING BELT ANIMATIONS
    IEnumerator AnimateOpen() 
    {
        //StartCoroutine(FadeTo(1.0f, 1.0f));
		float step1time = openTime /3;
		float step2time = openTime - step1time;

		float angleIncrement = Mathf.Clamp (angularRange / numItems, minSepAngle, maxSepAngle);
		float trueRange = angleIncrement * (numItems-1); // TODO hide items outside the configured range

		// Show items
		if (state == BeltState.Open) {
			foreach (GameObject g in nodes) {
				g.SetActive (true);
			}
		}
		// Move item bundle outward
        for (int i = 0; i < items.Length; i++)
        {
			iTween.RotateTo (nodes [i], iTween.Hash ("y",  - trueRange/2, "time", step1time/4, "isLocal", true));
			iTween.MoveTo (items [i], iTween.Hash ("position", Vector3.forward * space + Vector3.up * raiseHeight, "time", step1time*1.5f, "isLocal", true));

        }

		yield return new WaitForSeconds(step1time);

		// Spread items across the main arc
		for (int i = 0; i < items.Length; i++) {
			iTween.RotateTo (nodes [i], iTween.Hash ("y", angleIncrement * i - trueRange/2, "time", step2time, "easetype", iTween.EaseType.easeInOutQuad, "isLocal", true));
			iTween.RotateTo (items [i], iTween.Hash ("x", pitchAngle, "time", step2time, "isLocal", true));
		}

		yield return new WaitForSeconds (step2time);
    }


    IEnumerator AnimateClose()
	{
		float angleIncrement = Mathf.Clamp (angularRange / numItems, minSepAngle, maxSepAngle);
		float trueRange = angleIncrement * (numItems-1); // TODO hide items outside the configured range

		//StartCoroutine(FadeTo(0.0f, 0.1f));
		float step1time = closeTime /2;
		float step2time = closeTime - step1time;


		// Collapse items into bundle
        for (int i = 0; i < nodes.Length; i++)
		{
			iTween.RotateTo(items[i], iTween.Hash("x",0.0f,"y",0.0f, "time", step1time, "isLocal", true));
			iTween.RotateTo (nodes [i], iTween.Hash ("y", -trueRange / 2, "time", step1time, "easetype", iTween.EaseType.easeInOutQuad, "isLocal", true));
        }

        yield return new WaitForSeconds(step1time);

		// Move item bundle inward
		for (int i = 0; i < items.Length; i++)
        {  
			iTween.MoveTo (items [i], iTween.Hash ("position", Vector3.zero, "time", step2time, "isLocal", true));
        }
		yield return new WaitForSeconds (step2time);

		// Hide items
		if (state == BeltState.Closed) {
			foreach (GameObject g in nodes) {
				g.SetActive (false);
			}
		}
    }

    //Fade Alpha Channel on belt icons
    IEnumerator FadeTo(float aValue, float aTime)
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
