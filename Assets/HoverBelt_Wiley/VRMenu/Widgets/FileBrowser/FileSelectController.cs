using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class FileSelectController : MonoBehaviour {

	public GameObject listItemPrefab;

	public VRMenuScroll scroll;
	public VRMenuButton confirmButton;
	public VRMenuButton cancelButton;
	public Text dirLabel;

	private FileSelectItemData currentDirData;
	private FileSelectItemController selectedItem;
	public Action<String> callback;

	private string initialDir;
	private bool returnDirectory;
	private string highlightExtension;

	public void Configure(Action<string> _callback, string _initialDir = null, bool _returnDirectory = false, string _highlightExtension = null) {

		callback = _callback;

		initialDir = String.IsNullOrEmpty(_initialDir) ? Application.dataPath : initialDir;
		returnDirectory = _returnDirectory;
		highlightExtension = _highlightExtension;

		confirmButton.interactable = false;

		confirmButton.OnClick += HandleConfirm;
		cancelButton.OnClick += HandleCancel;

		DisplayItems (initialDir);


	}

	public void HandleItemStateChange(FileSelectItemController item, bool state) {
		if (item == selectedItem) {	
			HandleDeselect ();
		} else {
			HandleSelect (item);
		}
	}

	public void HandleSelect(FileSelectItemController item) {
		if (selectedItem != null) {
			selectedItem.state = false;
		}
		selectedItem = item;

		confirmButton.interactable = true;
	}

	public void HandleDeselect() {
		selectedItem = null;
		confirmButton.interactable = false;
	}

	public void HandleDoubleClick(VRMenuEventData e) {
		if (selectedItem == null) {
			return;
		}
		if (selectedItem.data.isDir) {
			DisplayItems (selectedItem.data.path);
		} else {
			HandleConfirm (e);
		}
	}

	public void HandleConfirm(VRMenuEventData e) {
		if (selectedItem == null) {
			if (returnDirectory) {
				callback (currentDirData.path);
			}
			return;
		}
		if (selectedItem.data.isDir == returnDirectory) {
			callback (selectedItem.data.path);
		}
	}

	public void HandleCancel(VRMenuEventData e) {
		callback (null);
	}

	private void ClearItems() {
		confirmButton.enabled = false;
		selectedItem = null;
		scroll.Clear (true);
	}

	private void DisplayItems(string dir) {
		ClearItems ();

		FileSelectItemData[] itemsData = GetData (dir, !returnDirectory, true);

		foreach (FileSelectItemData itemData in itemsData) {
			GameObject listItemObject = (GameObject)Instantiate (listItemPrefab);
			FileSelectItemController listItem = listItemObject.GetComponent<FileSelectItemController> ();
			listItem.Configure (itemData);
			listItem.OnStateChange += ((bool state) => HandleItemStateChange(listItem,state));
			listItem.OnDoubleClick += HandleDoubleClick;

			scroll.AddItem (listItem.gameObject);
		}

		currentDirData = new FileSelectItemData (dir, true);
		confirmButton.interactable = returnDirectory;


		//StartCoroutine (VRMenuItem.RecomputeCollidersNextFrame (items.ToArray ()));

		dirLabel.text = dir;
	}

	private static FileSelectItemData[] GetData(string path, bool includeFiles = true, bool includeDirectories = true) {
		List<FileSelectItemData> data = new List<FileSelectItemData>();

		Debug.Log (path);

		if (includeDirectories) {
			DirectoryInfo parent = Directory.GetParent (path);
			if (parent != null) {
				data.Add (new FileSelectItemData (parent.FullName, true, ".."));
			}

			string[] directories = Directory.GetDirectories (path);
			for (int i = 0; i < directories.Length; i++) {
				data.Add (new FileSelectItemData (directories [i], true));
			}
		}

		if (includeFiles) {
			string[] files = Directory.GetFiles (path);
			for (int i = 0; i < files.Length; i++) {
				data.Add (new FileSelectItemData (files [i], false));
			}
		}

		return(data.ToArray());
	}
}
