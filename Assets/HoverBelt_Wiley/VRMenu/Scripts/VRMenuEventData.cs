using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class VRMenuEventData {
	public enum InteractionType {
		Raycast,
		Collider
	}
	public string deviceType;
	public InteractionType interactionType;
	public GameObject originator;

	public static VRMenuEventData FromVive(SteamVR_TrackedObject obj) {
		VRMenuEventData data = new VRMenuEventData ();
		data.deviceType = "Vive";
		data.interactionType = InteractionType.Collider;
		data.originator = obj.gameObject;
		return data;
	}
}
