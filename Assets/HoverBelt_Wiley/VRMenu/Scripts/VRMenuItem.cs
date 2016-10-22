using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// A generic menu item with a collider, suitable for use with gaze controls.
public class VRMenuItem : MonoBehaviour {
	public delegate void VRMenuEvent (VRMenuEventData e);

	public MeshRenderer modelRenderer;
	public Canvas frontDisplayCanvas;

	protected Image image;
	protected Text text;

	public virtual void Start() {

		// Get model object if it exists
		// We iterate here so as not to accidentally retrieve the model of a child VRMenuItem
		if (modelRenderer == null) {
			foreach (Transform immediateChildTransform in transform) {
				MeshRenderer meshRenderer = immediateChildTransform.GetComponent<MeshRenderer> ();
				if (meshRenderer != null) {
					meshRenderer = meshRenderer;
					break;
				}
			}
		}


		// Get front display object if it exists
		// We iterate here so as not to accidentally retrieve the front display of a child VRMenuItem
		if (frontDisplayCanvas == null) {
			foreach (Transform immediateChildTransform in transform) {
				Canvas canvas = immediateChildTransform.GetComponent<Canvas> ();
				if (canvas != null) {
					frontDisplayCanvas = canvas;
					break;
				}
			}
		}

		// Get front display image / text if they exist
		if (frontDisplayCanvas != null) {
			image = frontDisplayCanvas.GetComponentInChildren<Image> ();
			text = frontDisplayCanvas.GetComponentInChildren<Text> ();
		}

		//RecomputeCollider ();
	}
	/*
	public virtual void RecomputeCollider() {
		RectTransform t = GetComponent<RectTransform> ();
		if (t == null) {
			return;
		}

		BoxCollider boxCollider = GetComponent<BoxCollider> ();
		if (boxCollider == null) {
			boxCollider = gameObject.AddComponent<BoxCollider> ();
		}
		boxCollider.size = new Vector3 (t.rect.width, t.rect.height, 0.1f);
		//boxCollider.center = new Vector3 (-(t.rect.xMax - t.rect.xMin) / 2, -(t.rect.yMax - t.rect.yMin) / 2);
	}

	public static IEnumerator RecomputeCollidersNextFrame(VRMenuItem[] items) {
		yield return new WaitForEndOfFrame ();
		foreach (VRMenuItem item in items) {
			item.RecomputeCollider ();
		}
	}*/

	public event VRMenuEvent OnIn;
	public event VRMenuEvent OnOut;
	public event VRMenuEvent OnClick;
	public event VRMenuEvent OnDoubleClick;
	public event VRMenuEvent OnPointerDown;
	public event VRMenuEvent OnPointerUp;

	public virtual void NotifyIn 	 	  (VRMenuEventData e = null) {if(OnIn          != null){OnIn          (e);}}
	public virtual void NotifyOut	 	  (VRMenuEventData e = null) {if(OnOut         != null){OnOut         (e);}}
	public virtual void NotifyClick 	  (VRMenuEventData e = null) {if(OnClick       != null){OnClick       (e);}}
	public virtual void NotifyDoubleClick (VRMenuEventData e = null) {if(OnDoubleClick != null){OnDoubleClick (e);}}
	public virtual void NotifyPointerDown (VRMenuEventData e = null) {if(OnPointerDown != null){OnPointerDown (e);}}
	public virtual void NotifyPointerUp   (VRMenuEventData e = null) {if(OnPointerUp   != null){OnPointerUp   (e);}}
}
