using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class KeypadController : MonoBehaviour {

	private string _data = "";

	public string displayText {
		get {
			if (data.Length > 0) {
				return data;
			} else {
				return placeholder;
			}
		}
	}

	public string data {
		get {
			return _data;
		}
		protected set {
			_data = value;
			if (valueLabel != null) {
				if (_data.Length > 0) {
					valueLabel.text = data;
				} else {
					valueLabel.text = placeholder;
				}
			}
			if (OnValueChange != null) {
				OnValueChange ();
			}

		}
	}


	public event Action OnValueChange;
	public event Action OnSetChange;

	public VRMenuButton backspaceButton;
	public VRMenuButton clearButton;

	public VRMenuButton confirmButton;
	public VRMenuButton cancelButton;

	public VRMenuCheckbox shiftButton;
	public VRMenuCheckbox numButton;

	public bool shiftPressed = false;
	public bool numPressed = false;

	public Text valueLabel;

	private string placeholder;
	public int maxLength { get; protected set; }
	private Regex regex;


	public Action<String> callback;




	public void Configure(Action<string> _callback, string _placeholder = null, int _maxLength = 100, string _regexPattern = null){
		callback = _callback;

		placeholder = _placeholder ?? "";
		maxLength   = _maxLength;
		regex       = String.IsNullOrEmpty(_regexPattern) ? null : new Regex(_regexPattern);

		if (valueLabel != null) {
			valueLabel.text = placeholder;
		}

		backspaceButton.OnClick += HandleBackspace;
		clearButton.OnClick     += HandleReset;
		confirmButton.OnClick   += HandleConfirm;
		cancelButton.OnClick    += HandleCancel;

		shiftButton.OnStateChange += ((bool state) => ChangeSet(state,numButton.state));
		numButton.OnStateChange += ((bool state) => ChangeSet(shiftButton.state,state));

		OnValueChange += RefreshConfirm;
		OnValueChange ();
	}

	public void Register(KeypadKeyController key) {
		key.OnClick += ((VRMenuEventData e) => HandleKey (key.keyString));
	}

	public void HandleKey(string keyString) {
		Debug.Log (keyString);
		if (data.Length + keyString.Length <= maxLength) {
			data = data + keyString;
		}
	}

	public void HandleBackspace(VRMenuEventData e) {
		if (data.Length == 0) {
			return;
		}
		data = data.Substring (0, data.Length - 1);
	}

	public void HandleReset(VRMenuEventData e) {
		data = "";
	}

	public void ChangeSet(bool _shiftPressed,bool _numPressed) {
		shiftPressed = _shiftPressed;
		numPressed = _numPressed;
		if (OnSetChange != null) {
			OnSetChange ();
		}
	}

	public void RefreshConfirm() {
		confirmButton.interactable = TestInput ();
	}

	public bool TestInput() {
		return (regex != null ? regex.IsMatch (data) : true);
	}

	public void HandleConfirm(VRMenuEventData e) {
		callback (data);
	}

	public void HandleCancel(VRMenuEventData e) {
		callback (null);
	}
}
