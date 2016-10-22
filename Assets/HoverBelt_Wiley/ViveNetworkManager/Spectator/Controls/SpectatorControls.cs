using UnityEngine;
using System;


public abstract class SpectatorControls {

	protected SpectatorController me;

	protected event Action OnClick;
	protected event Action OnDoubleClick;
	protected event Action OnPointerUp;
	protected event Action OnPointerDown;

	private float prevDown = Mathf.NegativeInfinity;
	private float lastDown = Mathf.NegativeInfinity;
	private float lastUp = Mathf.NegativeInfinity;

	protected bool isPointerDown = false;

	private static readonly float clickDuration = 0.5f;
	private static readonly float doubleClickPauseDuration = 0.2f;

	public SpectatorControls(SpectatorController _me) {
		me = _me;
	}

	public virtual void HandleInput() {
		float now = Time.time;

		if (Input.GetMouseButtonDown (0)) {
			if (OnPointerDown != null) {OnPointerDown ();}
			prevDown = lastDown;
			lastDown = now;
			isPointerDown = true;
		}

		if (Input.GetMouseButtonUp(0)) {
			if (OnPointerUp != null) {OnPointerUp ();}

			// Detect click
			if (now - lastDown <= clickDuration) {

				// Detect double click
				if((lastUp - prevDown <= clickDuration) && (lastDown - lastUp <= doubleClickPauseDuration)) {
					// Handle double click
					if(OnDoubleClick != null ) {OnDoubleClick ();}
				}
				else {
					// Handle single click
					if(OnClick != null ) {OnClick ();}
				}
			}
			lastUp = now;
			isPointerDown = false;
		}
	}

	public virtual void Transition(SpectatorControls nextControls) {
		OnClick = null;
		OnDoubleClick = null;
		OnPointerUp = null;
		OnPointerDown = null;

		me.SetControls(nextControls);
	}
}

