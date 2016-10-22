using UnityEngine;
using System.Collections;

public class SettingsMenu : MonoBehaviour {
	public VRMenuTextInput displayNameField;
	public SpectatorController controller;

	// Use this for initialization
	void Start () {
		if (controller == null) {
			controller = GetComponentInParent<SpectatorController> ();
		}
		displayNameField.OnValueSet += SetDisplayName;
	}

	private void SetDisplayName(string _displayname) {
		controller.displayName = _displayname;
	}
}
