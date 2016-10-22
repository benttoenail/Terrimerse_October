using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRMenuScroll : MonoBehaviour {
	public VRMenuButton scrollUpButton;
	public VRMenuButton scrollDownButton;

	public float interval;
	protected const float scrollRate = 5.0f;
	protected List<GameObject> scrollItems = new List<GameObject>();

	protected bool isScrollUp = false;
	protected bool isScrollDown = false;

	// Use this for initialization
	void Start () {
		scrollUpButton.OnPointerDown   += ((VRMenuEventData e) => isScrollUp   = true );
		scrollUpButton.OnPointerUp     += ((VRMenuEventData e) => isScrollUp   = false);
		scrollDownButton.OnPointerDown += ((VRMenuEventData e) => isScrollDown = true );
		scrollDownButton.OnPointerUp   += ((VRMenuEventData e) => isScrollDown = false);
	}

	public void Clear(bool destroyOld = true) {

		if (destroyOld) {
			foreach (GameObject item in scrollItems) {
				Destroy (item);
			}
		}

		scrollItems = new List<GameObject> ();
	}

	public void AddItems(GameObject[] items) {
		foreach (GameObject item in items) {
			AddItem (item);
		}
	}

	public void AddItem(GameObject obj) {
		obj.transform.SetParent (this.transform, false);
		if (scrollItems.Count == 0) {
			obj.transform.localPosition = scrollUpButton.transform.localPosition + Vector3.down * interval;
		}
		else {
			obj.transform.localPosition = scrollItems [scrollItems.Count - 1].transform.localPosition + Vector3.down * interval;
		}
		obj.SetActive (CheckInBounds (obj));
		scrollItems.Add (obj);
	}

	void Update() {
		if (isScrollUp ^ isScrollDown) {
			float increment = 0;
			if (isScrollUp) {
				increment += scrollRate * Time.deltaTime;
			}
			if (isScrollDown) {
				increment -= scrollRate * Time.deltaTime;
			}
			Scroll(increment);
		}
	}

	void Scroll(float increment) {
		Debug.Log (increment);
		foreach (GameObject obj in scrollItems) {
			obj.transform.localPosition += Vector3.down * increment;
			obj.SetActive (CheckInBounds(obj));
		}
	}

	bool CheckInBounds(GameObject obj) {
		return (obj.transform.localPosition.y > scrollDownButton.transform.localPosition.y) && (obj.transform.localPosition.y < scrollUpButton.transform.localPosition.y);
	}
}
