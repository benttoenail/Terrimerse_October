using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaybackMenu : MonoBehaviour {
	public GameObject recordTab;
	public VRMenuButton replayTabButton;
	public VRMenuFilenameInput recordDirectory;
	public VRMenuTextInput recordFilename;
	public VRMenuButton recordStartButton;
	//public GazeMenuCheckbox recordPauseButton;
	public VRMenuButton recordStopButton;
	public Text recordTime;



	public GameObject replayTab;
	public VRMenuButton recordTabButton;
	public VRMenuFilenameInput replayFile;
	public VRMenuButton replayGetPrevious;
	public VRMenuButton replayStartButton;
	//public GazeMenuCheckbox replayPauseButton;
	public VRMenuButton replayStopButton;
	public Text replayTime;
	//public Text replayLength;

	public SpectatorController controller;

	private string lastRecordedFile;
	private float recordStartTime;
	private float replayStartTime;

	private bool _isRecording;
	public bool isRecording {
		get { return _isRecording; }
		protected set {
			_isRecording = value;
			recordStartButton.interactable = !_isRecording;
			//recordPauseButton.interactable = !_isRecording;
			recordStopButton.interactable = _isRecording;
			replayTabButton.interactable = !_isRecording;
		}
	}

	private bool _isReplaying;
	public bool isReplaying {
		get { return _isReplaying; }
		protected set {
			_isReplaying = value;
			replayStartButton.interactable = !_isReplaying;
			//replayPauseButton.interactable = !_isReplaying;
			replayStopButton.interactable = _isReplaying;
			recordTabButton.interactable = !_isReplaying;
		}
	}



	// Use this for initialization
	void Start () {
		if (controller == null) {
			controller = GetComponentInParent<SpectatorController> ();
		}

		// Currently only the server player can perform playback functionality
		if (!controller.isServer) {
			Destroy (gameObject);
			return;
		}

		isRecording = false;
		isReplaying = false;

		recordStartButton.OnClick += StartRecording;
		recordStopButton.OnClick += StopRecording;
		replayStartButton.OnClick += StartReplaying;
		replayStopButton.OnClick += StopReplaying;

		replayTabButton.OnClick += OpenReplayTab;
		recordTabButton.OnClick += OpenRecordTab;

		replayGetPrevious.interactable = false;
		replayGetPrevious.OnClick += GetPrevious;
	}
	
	// Update is called once per frame
	void Update () {
		if (isRecording) {
			recordTime.text = (Time.time - recordStartTime).ToString();
		} else if (isReplaying) {
			replayTime.text = (Time.time - replayStartTime).ToString();
		}
	}

	void GetPrevious(VRMenuEventData e) {
		replayFile.inputValue = lastRecordedFile;
	}

	void OpenRecordTab(VRMenuEventData e) {
		replayTab.SetActive (false);
		recordTab.SetActive (true);
	}
	void OpenReplayTab(VRMenuEventData e) {
		recordTab.SetActive (false);
		replayTab.SetActive (true);
	}

	void StartReplaying(VRMenuEventData e) {
		if (isRecording || isReplaying) {
			return;
		}
		// TODO exception handling
		controller.networkFunctions.CmdStartReplay (replayFile.inputValue); 
		replayStartTime = Time.time;
		isReplaying = true;

	}
	void StopReplaying(VRMenuEventData e) {
		if (!isReplaying) {
			return;
		}
		// TODO exception handling
		controller.networkFunctions.CmdStopReplay ();
		isReplaying = false;
		replayTime.text = "";
	}

	void StartRecording(VRMenuEventData e) {
		Debug.LogFormat ("{0} || {1} -> no", isRecording, isReplaying);
		if (isRecording || isReplaying) {
			return;
		}
		// TODO exception handling
		lastRecordedFile = recordDirectory.inputValue + '/' + recordFilename.inputValue;
		controller.networkFunctions.CmdStartRecording (lastRecordedFile); 
		recordStartTime = Time.time;
		isRecording = true;

	}
	void StopRecording(VRMenuEventData e) {
		if (!isRecording) {
			return;
		}
		controller.networkFunctions.CmdStopRecording ();
		isRecording = false;
		recordTime.text = "";
		replayGetPrevious.interactable = true;
	}
}
