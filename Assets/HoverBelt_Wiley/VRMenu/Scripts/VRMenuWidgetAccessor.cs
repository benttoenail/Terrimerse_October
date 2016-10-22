using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A checkbox that, on click, opens a widget.
public class VRMenuWidgetAccessor : VRMenuCheckbox {

	public GameObject widgetPrefab;
	public VRMenuWidgetMutex mutex;

	public override void Start() {
		base.Start ();
		OnStateChange += HandleStateChange;
		mutex.OnMutexHeldChange += HandleMutexHeldChange;
	}

	protected void HandleStateChange(bool newState) {
		if (!newState) {
			mutex.Release ();
		} else {
			GameObject widgetInstance = mutex.Request (this, widgetPrefab);
			if (widgetInstance != null) {
				ConfigureWidget (widgetInstance);
			}
		}
	}

	protected virtual void ConfigureWidget(GameObject widgetInstance) {	}

	void HandleMutexHeldChange(VRMenuWidgetAccessor holder) {
		interactable = (holder == null || holder == this);
	}

	void HandleWidgetClose() {
		mutex.Release ();
		this.state = false;
	}
}
