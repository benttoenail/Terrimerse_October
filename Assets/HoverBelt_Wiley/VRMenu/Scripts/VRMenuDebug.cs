using UnityEngine;
using System.Collections;

public class VRMenuDebug : VRMenuItem {
	private MeshRenderer r;

	void Start() {
		r = GetComponent<MeshRenderer> ();
		OnIn += HandleIn;
		OnOut += HandleOut;
		OnClick += HandleClick;
		OnDoubleClick += HandleDoubleClick;
	}

	public void HandleIn(VRMenuEventData e) {
		React (Color.black);
	}
	public void HandleOut(VRMenuEventData e) {
		React (Color.white);
	}
	public void HandleClick(VRMenuEventData e) {
		React(Color.green);
	}
	public void HandleDoubleClick(VRMenuEventData e) {
		React (Color.blue);
	}
	private void React(Color c) {
		if (r != null) {
			r.material.color = c;
		}
	}
}
