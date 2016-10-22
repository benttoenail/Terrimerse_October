using UnityEngine;
using System.Collections;
using System;

// A mutual exclusion handler, intended for use with temporary menu components.
// Not thread-safe.
public class VRMenuWidgetMutex : MonoBehaviour {


	private VRMenuWidgetAccessor holder;

	private GameObject occupant;

	public event Action<VRMenuWidgetAccessor> OnMutexHeldChange;

	public GameObject Request(VRMenuWidgetAccessor requester, GameObject prefab) {
		Debug.Log ("REQUEST");
		if (holder == null) {
			
			holder = requester;

			occupant = (GameObject)Instantiate (prefab);
			occupant.transform.SetParent(transform,false);

			//StartCoroutine (VRMenuItem.RecomputeCollidersNextFrame(occupant.GetComponentsInChildren<VRMenuItem>()));

			if (OnMutexHeldChange != null) {
				Debug.Log ("EMIT(true)");
				OnMutexHeldChange (holder);
			}

			return occupant;
		}
		return null;
	}

	public void Release() {
		Debug.Log ("RELEASE");
		holder = null;

		if (occupant != null) {
			Destroy (occupant);
		}

		if (OnMutexHeldChange != null) {
			Debug.Log ("EMIT(false)");
			OnMutexHeldChange (null);
		}
	}
}
