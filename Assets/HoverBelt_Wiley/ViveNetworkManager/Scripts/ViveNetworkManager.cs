using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ViveNetworkManager : NetworkManager {
	public bool hostOnStart = true;
	public static ViveNetworkManager vnmSingleton;

	public GameObject vivePlayerPrefab;
	public GameObject spectatorPrefab;


    // Use this for initialization
    void Start () {


		if (vnmSingleton != null) {
			Debug.LogWarning ("Multiple ViveNetworkManagers detected.");
			//Destroy (gameObject);
			//return;
		}
		vnmSingleton = this;

        GameObject ViveTrackingObjectPrefab = (GameObject)Resources.Load("ViveTrackingPoint");
        // Add the ViveTrackingObject to the list of spawnable prefabs.
        if (!spawnPrefabs.Contains(ViveTrackingObjectPrefab)) {
            spawnPrefabs.Add(ViveTrackingObjectPrefab);
        }

		if (!spawnPrefabs.Contains (vivePlayerPrefab)) {
			spawnPrefabs.Add (vivePlayerPrefab);
		}
		if (!spawnPrefabs.Contains (spectatorPrefab)) {
			spawnPrefabs.Add (spectatorPrefab);
		}
        // Add the ViveAvatar component to the player prefab if it's not already there.
        // Note, this addes it permanently. Alternatively I could just issue an error, or add in spawn time.
        if (vivePlayerPrefab.GetComponent<ViveAvatar>() == null)
        {
			vivePlayerPrefab.AddComponent<ViveAvatar>();
        }
		if (vivePlayerPrefab.GetComponent<ViveNetworkFunctions>() == null)
        {
			vivePlayerPrefab.AddComponent<ViveNetworkFunctions>();
        }

		if (hostOnStart) {
			StartHost ();
		}
    }

    // Update is called once per frame
	void Update () {
		
	}

	/* TODO refactor */
	public void UnhostAndConnect() {
		StartCoroutine (_UnhostAndConnect());
	}

	private IEnumerator _UnhostAndConnect() {
		StopHost ();

		yield return new WaitForSeconds (0.1f);

		StartClient ();

		yield return StartCoroutine (CheckConnection());
	}

	public IEnumerator CheckConnection() {
		yield return new WaitForSeconds (15);
		if(!isNetworkActive) {
			Debug.Log ("Connection failed");
			StopClient ();
			if (hostOnStart) {
				yield return StartCoroutine (Rehost());
			}
		}
	}

	public void DoDisconnect() {
		StopClient ();
		if (hostOnStart) {
			StartCoroutine (Rehost ());
		}
	}

	private IEnumerator Rehost() {
		yield return new WaitForSeconds (0.5f);
		Debug.Log ("Rehosting");
		networkAddress = "127.0.0.1";
		networkPort = 7777;
		StartHost ();
	}
}
