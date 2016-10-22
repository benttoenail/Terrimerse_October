using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// A text field that, on click, opens a text input widget.
public class VRMenuTextInput : VRMenuWidgetAccessor {
	
	private string _inputValue;
	public string inputValue {
		get {return _inputValue;}

		set{
			_inputValue = value;
			if (text != null) {
				text.text = value;
			}
			if (OnValueSet != null) {
				OnValueSet (value);
			}
		}
	}

	public string placeholder = null;
	public int maxLength = 100;
	public string regexPattern = null;

	public event Action<string> OnValueSet;

	public override void Start() {
		base.Start ();
	}

	public void SetInitialValue(string s) {
		StartCoroutine (_SetInitialValue (s));
	}
	private IEnumerator _SetInitialValue(string s) {
		yield return new WaitForEndOfFrame ();
		inputValue = s;
	}

	protected override void ConfigureWidget(GameObject widgetInstance) {

		KeypadController keypadController = widgetInstance.GetComponent<KeypadController> ();
		if (keypadController == null) {
			throw new MissingComponentException ("Missing input widget component");
		}
		keypadController.Configure (ReceiveInput,placeholder,maxLength,regexPattern);
	}

	private void ReceiveInput(string s) {

		if (s != null) {
			inputValue = s;
		}
		
		mutex.Release ();
		this.state = false;
	}
}
