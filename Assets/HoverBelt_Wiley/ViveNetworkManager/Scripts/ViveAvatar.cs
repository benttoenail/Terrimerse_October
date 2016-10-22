using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

// This is the main component of the player object giving it all the vive networking capabilities
public class ViveAvatar : NetworkBehaviour {

    // Constants
    public const int HEAD = 0;
    public const int LEFT = 1;
    public const int RIGHT = 2;

    // Static function to find a ViveAvatar object
    public static ViveAvatar me { get { return localViveAvatar; } } // should be used to return the "me" ViveAvatar
    public static List<ViveAvatar> avatars = new List<ViveAvatar>(); // The list of all active ViveAvatars. These include "me" and all proxy avatars

    // Controllers. Access using above constants.
    public ViveController[] controllers = new ViveController[CONTROLLER_COUNT];

    // Flags
    public bool isLocalVive; // Flag that should be used to determine if this is the localVive
    public bool isGhost = false; //True for ghost players that exist during playback
    public bool hideDuringPlayback = true; // This flag controls whether the avatar and the controllers are disabled (hidden) during playback
    public bool initialized = false;

    // Used internally
    const int CONTROLLER_COUNT = 3; //head left right
    static ViveAvatar localViveAvatar = null;

    // These syncvars are to set the tracking objects in proxy players
    [SyncVar] bool remoteInitialized = false;
    [SyncVar] GameObject viveTOHead;
    [SyncVar] GameObject viveTOLeft;
    [SyncVar] GameObject viveTORight;


    // Return the ViveController object that is a parent of the given obj.
    // Return null if obj is not a child of any of the current ViveAvatar's controllers.
    public ViveController findControllerFromChild(GameObject obj)
    {
        for (int i = 0; i < CONTROLLER_COUNT; i++)
        {
            if (obj.transform.IsChildOf(controllers[i].transform)) return controllers[i];
        }
        return null;
    }

    [ClientRpc]
    public void RpcSendMessage(string message)
    {
        gameObject.SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }

    [Server]
    static public void SendMessageToAllAvatars(string message)
    {
        foreach (var avatar in ViveAvatar.avatars)
        {
            if (!avatar.isGhost) avatar.RpcSendMessage(message);
        }
    }





    // This function adds an avatar to the currect gameobject (assumed to be a player object).
    [Command]
    public void CmdSpawnAvatar()
    {
        SpawnAvatar();
    }

    public void SpawnAvatar()
    {
        GameObject ViveTrackingObjectPrefab = Resources.Load<GameObject>("ViveTrackingPoint");
        GameObject[] goArray = new GameObject[CONTROLLER_COUNT];
        for (int i = 0; i < goArray.Length; i++)
        {
            GameObject go = (GameObject)Instantiate(ViveTrackingObjectPrefab);
            if (base.connectionToClient == null)
                 NetworkServer.Spawn(go);
            else NetworkServer.SpawnWithClientAuthority(go, base.connectionToClient);

            goArray[i] = go;
        }

        viveTOHead = goArray[HEAD];
        viveTOLeft = goArray[LEFT];
        viveTORight = goArray[RIGHT];
        remoteInitialized = true;

        // We have to send the tracking objects directly because the sync vars are not guaranteed to be synced by the time the RPC is called.
        // We sync them anyway for later clients that connect.
        // This can be optimized by waiting for the syncvars to arrive on the client.
        RpcSetupTO(goArray);
    }

    [ClientRpc]
    public void RpcSetGhost() { isGhost = true; }

    [ClientRpc]
    void RpcSetupTO(GameObject[] viveTrackingObjects)
    {
        Setup(viveTrackingObjects);
    }


    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        // This is called on the client when the player object is created on the local client
            CmdSpawnAvatar();
    }

    public override void OnStartClient()
    {
        // This could be called for a local client that hasn't been initialized yet, or on proxy for a remote client that has.
        // if its a proxy, (remote initlized) we are guaranteed that the syncvars are set, so use them to connect the controllers.
        // Otherwise, wait for the RPC call for setup.
        if (remoteInitialized) {
            GameObject[] goArray = new GameObject[CONTROLLER_COUNT];
            goArray[HEAD] = viveTOHead;
            goArray[LEFT] = viveTOLeft;
            goArray[RIGHT] = viveTORight;
            Setup(goArray);
        }

    }

    // This function sets up the vive controller objects
    void Setup(GameObject[] viveTrackingObjects)
    {
        if (initialized) return;

        // There can be only one localVive player. So check if localViveAvatar is set.
        isLocalVive = gameObject.GetComponent<NetworkBehaviour>().hasAuthority && !localViveAvatar;

        // Create the controllers
        for (int i = 0; i < CONTROLLER_COUNT; i++)
        {
            controllers[i] = gameObject.AddComponent<ViveController>();// new ViveController();
            controllers[i].transform = viveTrackingObjects[i].transform;
            controllers[i].viveAvatar = this;
            controllers[i].index = i;
        }

        // If its the local vive, attach the tracking objects to the steamVR controller objects
        if (isLocalVive)
        {
            localViveAvatar = this;
            GameObject cameraRig = GameObject.Find("[CameraRig]");
            GameObject[] viveActuals = new GameObject[3];
            viveActuals[HEAD] = cameraRig.transform.Find("Camera (eye)").gameObject;
            viveActuals[LEFT] = cameraRig.transform.Find("Controller (left)").gameObject;
            viveActuals[RIGHT] = cameraRig.transform.Find("Controller (right)").gameObject;

            for (int i = 0; i < CONTROLLER_COUNT; i++)
            {
                controllers[i].localGameObject = viveActuals[i];
                controllers[i].transform.SetParent(viveActuals[i].transform, false);
            }
        }

        // notify others that we are ready
        gameObject.SendMessage("OnViveReady", this, SendMessageOptions.DontRequireReceiver);
        initialized = true;

    }

    public void DestroyAvatar()
    {
        if (isServer)
        {
            NetworkServer.Destroy(viveTOHead);
            NetworkServer.Destroy(viveTOLeft);
            NetworkServer.Destroy(viveTORight);
            NetworkServer.Destroy(gameObject);
        }

    }

    public void Start()
    {
        avatars.Add(this);
    }

    public void OnDestroy()
    {
        avatars.Remove(this);
    }

    //----- Log position on controllers
    const string LOG_TRANSFORMS = "_settransforms";


    void Update()
    {
        // Log positions of the 3 tracked objects
        if (initialized && isServer && CommandLogger.isLogging)
        {
            CommandLogger.Log(new LogEntry(this, "", LOG_TRANSFORMS,
                new object[] {
                    controllers[ViveAvatar.HEAD].ToString(),
                    controllers[ViveAvatar.LEFT].ToString(),
                    controllers[ViveAvatar.RIGHT].ToString() },
                true));
        }
    }

    [Server]
    public void OnParseLogCommand(LogEntry cmd)
    {
        switch (cmd.methodName)
        {
            case LOG_TRANSFORMS:
                controllers[ViveAvatar.HEAD].fromString((string)cmd.parameters[0]);
                controllers[ViveAvatar.LEFT].fromString((string)cmd.parameters[1]);
                controllers[ViveAvatar.RIGHT].fromString((string)cmd.parameters[2]);
                break;
        }

    }

    [ClientRpc]
    public void RpcSetVisibility(bool show)
    {
        for (int i = 0; i < CONTROLLER_COUNT; i++)
        {
            controllers[i].transform.gameObject.SetActive(show);
        }
    }

    [Server]
    void OnStartReplay()
    {
        if (!hideDuringPlayback) return;

        RpcSetVisibility(false);

    }

    [Server]
    void OnStopReplay()
    {
        if (!hideDuringPlayback) return;

        RpcSetVisibility(true);
    }

}
