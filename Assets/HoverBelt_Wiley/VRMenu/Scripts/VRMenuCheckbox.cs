using UnityEngine;
using System;

// A toggle-style menu field. Appears as a button despite the name.
public class VRMenuCheckbox : VRMenuHoverItem {
	protected static readonly Color modelColorStateTrue = new Color (0.2f, 0.2f, 1, 0.8f);
	protected static readonly Color displayColorStateTrue  = new Color (0,0,0);
	protected static readonly float scaleFactorStateTrue = 0.95f;

	private bool _state = false;
	public bool state {
		get { return _state; }

		set {
			if (value) {
				SetModelColor (modelColorStateTrue);
				SetDisplayColor (displayColorStateTrue);
				transform.localScale *= scaleFactorStateTrue;
			} else {
				SetModelColor (modelColorInteractableTrue);
				SetDisplayColor (displayColorInteractableTrue);
				transform.localScale *= scaleFactorStateTrue;
			}
			if (OnStateChange != null) {
				OnStateChange (value);
			}
			_state = value;
		}
	}

	public event Action<bool> OnStateChange;

	public override void Start() {
		base.Start();
		OnPointerDown += HandleToggle;
	}


	void HandleToggle(VRMenuEventData e) {
		state = !state;
	}
}
