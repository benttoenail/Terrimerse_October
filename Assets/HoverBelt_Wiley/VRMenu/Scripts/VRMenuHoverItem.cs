using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A generic menu item that gives visual feedback on hover,
// and whose interactibility can be enabled or disabled.
public abstract class VRMenuHoverItem : VRMenuItem {
	protected static readonly Color modelColorInteractableTrue = new Color (1, 1, 1, 0.8f);
	protected static readonly Color modelColorInteractableFalse = new Color (0.5f, 0.5f, 0.5f, 0.5f);
	protected static readonly Color displayColorInteractableTrue = new Color (1,1,1); // Starting Color?
	protected static readonly Color displayColorInteractableFalse = new Color (0.3f,0.3f,0.3f);

	protected static readonly float hoverAnimationLength = 0.2f;
	protected static readonly float forwardOffset = 0.0f;//-0.25f;
	protected static readonly float forwardScaleFactor = 1.25f;

	private Vector3 scaleBase;
	private Vector3 scaleForward;

	protected float forwardness = 0;

	private bool isHover = false;

	private bool _isForward = false;
	private bool isForward {
		get { 
			return _isForward;
		}
		set { 
			if (value != isForward) {
				_isForward = value;
				if (_isForward) {
					StartCoroutine (MoveForward());
				} else {
					StartCoroutine (MoveBackward());
				}
			}
		}
	}

	private bool _interactable = true;
	public bool interactable {
		get {
			return _interactable;
		}
		set {
			if (_interactable == value) {
				return;
			}

			_interactable = value;

			RefreshView ();
		}
	}


	public override void Start() {
		base.Start();
		scaleBase = transform.localScale;
		scaleForward = scaleBase * forwardScaleFactor;

		OnIn += HandleIn;
		OnOut += HandleOut;

		RefreshView ();
	}

	protected void SetDisplayColor(Color displayColor) {
		if (image != null) {image.color = displayColor; }
		if (text  != null) {text.color  = displayColor; }
	}
	protected void SetModelColor(Color modelColor) {
		if (modelRenderer != null) {modelRenderer.material.color = modelColor; }
	}


	private void RefreshView() {
		if (interactable) {
			SetModelColor (modelColorInteractableTrue);
			SetDisplayColor (displayColorInteractableTrue);
			if (isHover && !isForward) {
				isForward = true;
			}
		} else {
			SetModelColor (modelColorInteractableFalse);
			SetDisplayColor (displayColorInteractableFalse);
			if (isForward) {
				isForward = false;
			}
		}

	}

	private IEnumerator MoveForward() {
		float startTime = Time.time;
		float nowTime = Time.time;
		while (nowTime - startTime < hoverAnimationLength) {

			float forwardnessIncrement = Time.deltaTime / hoverAnimationLength;
			forwardness = Mathf.Min (forwardness + forwardnessIncrement, 1.0f);

			transform.localPosition += Vector3.forward * forwardnessIncrement * forwardOffset;
			transform.localScale = Vector3.Lerp (scaleBase, scaleForward, forwardness);

			yield return new WaitForEndOfFrame ();
			nowTime += Time.deltaTime;
		}
	}

	private IEnumerator MoveBackward() {
		float startTime = Time.time;
		float nowTime = Time.time;
		while (nowTime - startTime < hoverAnimationLength) {

			float forwardnessIncrement = Time.deltaTime / hoverAnimationLength;
			forwardness = Mathf.Max (forwardness - forwardnessIncrement, 0.0f);

			transform.localPosition -= Vector3.forward * forwardnessIncrement * forwardOffset;
			transform.localScale = Vector3.Lerp (scaleBase, scaleForward, forwardness);

			yield return new WaitForEndOfFrame ();
			nowTime += Time.deltaTime;
		}
	}

	public void HandleIn(VRMenuEventData e) {
		isHover = true;
		if (interactable) {
			isForward = true;
		}
	}
	public void HandleOut(VRMenuEventData e) {
		isHover = false;
		if (interactable) {
			isForward = false;
		}
	}

	// Do click events only if interactable
	public override void NotifyClick 	   (VRMenuEventData e = null) {if(interactable){base.NotifyClick       (e);}}
	public override void NotifyDoubleClick (VRMenuEventData e = null) {if(interactable){base.NotifyDoubleClick (e);}}
	public override void NotifyPointerDown (VRMenuEventData e = null) {if(interactable){base.NotifyPointerDown (e);}}
	public override void NotifyPointerUp   (VRMenuEventData e = null) {if(interactable){base.NotifyPointerUp   (e);}}
}
