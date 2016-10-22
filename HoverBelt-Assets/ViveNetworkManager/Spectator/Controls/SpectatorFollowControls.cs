using UnityEngine;
using System.Collections;

public class SpectatorFollowControls : SpectatorControls {

	private ViveController target;
	private int avatarIdx;

	public SpectatorFollowControls(SpectatorController _me, int _avatarIdx = 0) : base(_me) {
		avatarIdx = _avatarIdx;
		if(avatarIdx >= ViveAvatar.avatars.Count) {
			// Will exit on the next frame
			return;
		}
		else {
			target = ViveAvatar.avatars[avatarIdx].controllers[ViveAvatar.HEAD];
		}

		OnClick += ChangeView;
		OnDoubleClick += Exit;
	}

	public override void HandleInput() {

		if (target == null) {
			Exit ();
			return;
		}

		base.HandleInput();

		me.gameObject.transform.position = target.transform.position+Vector3.up*0.5f;
	}

	private void ChangeView() {
		if (ViveAvatar.avatars.Count == 0) {
			Exit ();
			return;
		}
		avatarIdx += 1;
		avatarIdx %= ViveAvatar.avatars.Count;
		target = ViveAvatar.avatars[avatarIdx].controllers[ViveAvatar.HEAD];
	}

	private void Exit() {
		Transition(new SpectatorHoverControls(me));
	}
}
