using UnityEngine;

public class SpectatorHoverControls : SpectatorControls {

	public SpectatorHoverControls(SpectatorController _me) : base(_me) {
		OnDoubleClick += OpenMenu;
	}

	public override void HandleInput() {
		base.HandleInput ();
		if (isPointerDown) {
			Move ();
		}
	}

	private void OpenMenu() {
		Transition (new SpectatorMenuControls (me));
	}

	public void Move() {
		me.transform.position += me.cam.transform.forward * me.flySpeed * Time.deltaTime;
	}
}