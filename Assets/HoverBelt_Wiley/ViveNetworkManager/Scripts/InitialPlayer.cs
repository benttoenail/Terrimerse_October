using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Valve.VR;
using UnityEngine.VR;

// When a player connects to the server, a prefab with this behaviour will be spawned initially.
// This behaviour determines what (if any) VR device is connected and replaces itself with the
// appropriate device-specific player.
using System;


public class InitialPlayer : NetworkBehaviour {

	public GameObject debugPrefab;

	enum PlayerType {
		Vive,
		Other
	}

	void Start () {
		if (isLocalPlayer) {
			try {
				if (OpenVR.IsHmdPresent ()) {
					CmdDeterminePlayer (PlayerType.Vive);
				} else {
					CmdDeterminePlayer (PlayerType.Other);
				}
			}
			catch(Exception e) {
				CmdDeterminePlayer (PlayerType.Other);
			}
		}
	}

	[Command]
	void CmdDeterminePlayer(PlayerType playerType) {
		CommandLogger.Log (this, playerType);

		NetworkConnection connection = this.GetComponent<NetworkIdentity> ().connectionToClient;
		short controllerId = this.GetComponent<NetworkIdentity> ().playerControllerId;

		GameObject replacementPlayerPrefab;

		if (playerType == PlayerType.Vive) {
			replacementPlayerPrefab = ViveNetworkManager.vnmSingleton.vivePlayerPrefab;
		} else {
			replacementPlayerPrefab = ViveNetworkManager.vnmSingleton.spectatorPrefab;
		}

		GameObject replacementPlayer = (GameObject)Instantiate (replacementPlayerPrefab);
		NetworkServer.ReplacePlayerForConnection (connection, replacementPlayer, controllerId);

		Destroy (gameObject);
	}
}
