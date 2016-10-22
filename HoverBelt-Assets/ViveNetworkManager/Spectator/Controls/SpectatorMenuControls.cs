using UnityEngine;
using System.Collections;

public class SpectatorMenuControls : SpectatorControls {
	private VRMenuItem item;
	private static readonly float defaultDist = 50;
	private static readonly float retSize = 0.5f;


	public SpectatorMenuControls(SpectatorController _me) : base(_me) {
		me.menuBubble.SetActive(true);
		me.menuBubble.transform.localRotation = Quaternion.Euler(0,me.cam.transform.rotation.eulerAngles.y,0);
		me.reticle.SetActive(true);

		//me.StartCoroutine (VRMenuItem.RecomputeCollidersNextFrame (me.menuBubble.GetComponentsInChildren<VRMenuItem>()));

		OnClick += HandleClick;
		OnPointerUp += HandlePointerUp;
		OnPointerDown += HandlePointerDown;
		OnDoubleClick += HandleDoubleClick;
	}

	public override void HandleInput() {

		Ray ray = new Ray(me.cam.transform.position,me.cam.transform.forward);
		RaycastHit hit;

		VRMenuItem newItem = null;

		if (Physics.Raycast (ray, out hit, 100)) {
			newItem = hit.collider.GetComponentInParent<VRMenuItem> ();
		}

		if (newItem != item) {
			if (item != null) {
				item.NotifyOut ();
			}
			if (newItem != null) {
				newItem.NotifyIn ();
			}
		}

		item = newItem;

		if (item != null) {
			me.reticle.transform.localPosition = new Vector3(0,0,hit.distance);
			float sc = retSize * hit.distance;
			me.reticle.transform.localScale = new Vector3 (sc, sc, sc);
		}
		else {
			me.reticle.transform.localPosition = new Vector3(0,0,defaultDist);
			float sc = retSize * defaultDist;
			me.reticle.transform.localScale = new Vector3 (sc,sc,sc);
		}

		base.HandleInput ();

	}

	private void HandleClick() {
		if (item != null) {
			item.NotifyClick ();
		} else {
			Exit ();
		}
	}

	private void HandleDoubleClick() {
		if (item != null) {
			item.NotifyDoubleClick ();
		} else {
			Exit ();
		}
	}

	private void HandlePointerDown() {
		if (item != null) {
			item.NotifyPointerDown ();
		} else {
			Exit ();
		}
	}
	private void HandlePointerUp() {
		if (item != null) {
			item.NotifyPointerUp ();
		} else {
			Exit ();
		}
	}

	private void Exit() {
		Transition(new SpectatorHoverControls(me));
	}

	public override void Transition(SpectatorControls nextControls) {

		me.menuBubble.SetActive(false);
		me.reticle.SetActive(false);

		base.Transition(nextControls);
	}
}
