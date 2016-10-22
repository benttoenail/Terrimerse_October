using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// A text field that, on click, opens a filename input widget.
public class VRMenuFilenameInput : VRMenuWidgetAccessor {

	private string _inputValue;
	public string inputValue {
		get {return _inputValue;}

		set{
			_inputValue = value.Replace ('\\', '/');
			if (text != null) {
				text.text = value;
			}
			if (OnValueSet != null) {
				OnValueSet (value);
			}
		}
	}
	
	public string initialDir = null;
	public bool returnDirectory = false;
	public string targetExtension = null;


	public event Action<string> OnValueSet;

	public override void Start() {
		base.Start ();
		if (inputValue == null) {
			inputValue = "";
		}
	}

	protected override void ConfigureWidget(GameObject widgetInstance) {

		FileSelectController fileSelectController = widgetInstance.GetComponent<FileSelectController> ();
		if (fileSelectController == null) {
			throw new MissingComponentException ("Missing input widget component");
		}
		fileSelectController.Configure (ReceiveInput,initialDir,returnDirectory,targetExtension);
	}

	private void ReceiveInput(string s) {

		if (s != null) {
			inputValue = s;
		}

		mutex.Release ();
		this.state = false;
	}
}
