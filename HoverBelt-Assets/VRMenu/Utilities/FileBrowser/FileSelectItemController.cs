using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileSelectItemController : VRMenuCheckbox {
	public FileSelectItemData data { get; private set; }
	private FileSelectController controller;

	private bool isSelected = false;

	public void Configure(FileSelectItemData _data) {
		data = _data;
		GetComponentInChildren<Text> ().text = data.name;
	}
	/*
	public override void RecomputeCollider() {
		RectTransform t = GetComponent<RectTransform> ();

		if (t == null) {
			return;
		}

		BoxCollider boxCollider = GetComponent<BoxCollider> ();
		if (boxCollider == null) {
			boxCollider = gameObject.AddComponent<BoxCollider> ();
		}

		boxCollider.size = new Vector3 (t.sizeDelta.x, t.sizeDelta.y, (t.sizeDelta.x + t.sizeDelta.y)*colliderDepthFactor*0.5f);
	}*/

}
