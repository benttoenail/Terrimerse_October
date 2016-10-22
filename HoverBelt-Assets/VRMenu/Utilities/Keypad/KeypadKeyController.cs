using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class KeypadKeyController : VRMenuButton {
	private KeypadController controller;
	public string primary = "";
	public string primaryCapital = "";
	public string secondary = "";
	public string secondaryCapital = "";

	public string keyString {
		get {
			return text.text;
		}
	}

	public override void Start() {
		base.Start ();
		text.text = primary;

		controller = GetComponentInParent<KeypadController> ();
		controller.OnValueChange += Refresh;
		controller.OnSetChange += Refresh;

		controller.Register (this);
	}

	private void Refresh() {
		if (controller.numPressed) {
			if (controller.shiftPressed) {
				text.text = secondaryCapital;
			} else {
				text.text = secondary;
			}
		} else {
			if (controller.shiftPressed) {
				text.text = primaryCapital;
			} else {
				text.text = primary;
			}
		}

		interactable = (text.text != "") && (text.text.Length + controller.data.Length <= controller.maxLength);

	}
}