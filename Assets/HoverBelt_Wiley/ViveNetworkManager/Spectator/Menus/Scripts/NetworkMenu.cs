using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkMenu : MonoBehaviour {
	public SpectatorController controller;

	public NetworkManager networkManager;
	public ViveNetworkManager vnm;

	public GameObject localHostingTab;
	//public GazeMenuInputField localPassword;
	//public GazeMenuInputField remotePassword;
	public VRMenuTextInput remoteIP;
	public VRMenuTextInput remotePort;
	public VRMenuButton remoteConnect;

	public GameObject remoteConnectedTab;
	public VRMenuButton remoteDisconnect;
	public Text remoteIPDisplay;
	public Text remotePortDisplay;


	public string localIP = "127.0.0.1";
	public int defaultPort = 7777;
	public string defaultIP = "";

	private bool _isLocal;
	public bool isLocal {
		get { return _isLocal; }
		private set {
			localHostingTab.SetActive (value);
			remoteConnectedTab.SetActive (!value);
			_isLocal = value;
		}
	}

	// Use this for initialization
	void Start () {
		if (controller == null) {
			controller = GetComponentInParent<SpectatorController> ();
		}
		if (networkManager == null) {
			networkManager = NetworkManager.singleton;
		}
		if (vnm == null) {
			vnm = networkManager.GetComponent<ViveNetworkManager> ();
		}

		// TODO better detection
		isLocal = controller.isServer;

		remoteConnect.OnClick += Connect;
		remoteDisconnect.OnClick += Disconnect;

		remotePort.SetInitialValue (defaultPort.ToString ());
		remoteIP.SetInitialValue (defaultIP);
	}

	void Connect(VRMenuEventData e) {
		remoteIPDisplay.text = remoteIP.inputValue;
		remotePortDisplay.text = remotePort.inputValue;

		networkManager.networkAddress = remoteIP.inputValue;
		networkManager.networkPort = int.Parse (remotePort.inputValue);

		isLocal = false;
		vnm.UnhostAndConnect ();
	}


	void Disconnect(VRMenuEventData e) {
		isLocal = true;
		vnm.DoDisconnect ();
	}
}
