using UnityEngine;

// A button. Changes color while held.
public class VRMenuButton : VRMenuHoverItem {
	protected static readonly Color modelColorHeld = new Color (0.2f, 0.2f, 1, 0.8f);
	protected static readonly Color displayColorHeld  = new Color (0,0,1); // Display color when clicked

	private static readonly float scaleFactorHeld = 0.95f;

	public override void Start() {
		base.Start ();
		OnPointerDown += HandlePointerDown;
		OnPointerUp += HandlePointerUp;

		// Convert double clicks to clicks
		OnDoubleClick += NotifyClick;
	}

	private void HandlePointerDown(VRMenuEventData e) {
		SetModelColor (modelColorHeld);
		SetDisplayColor (displayColorHeld);
		transform.localScale *= scaleFactorHeld;
	}
	private void HandlePointerUp(VRMenuEventData e) {
		SetModelColor (modelColorInteractableTrue);
		SetDisplayColor (displayColorInteractableTrue);
		transform.localScale /= scaleFactorHeld;
	}
}
