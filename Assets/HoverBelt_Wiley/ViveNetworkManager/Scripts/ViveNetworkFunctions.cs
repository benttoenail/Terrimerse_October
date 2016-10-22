using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class ViveNetworkFunctions : NetworkBehaviour {

    public bool isRecording = false;
    public bool isReplay = false;

    [Command]
    public void CmdStartRecording(string filename)
    {
        if (isRecording || isReplay) return;

        Debug.Log("Start Recording to " + filename);
        isRecording = true;
        CommandLogger.StartLogging(filename);
        ViveAvatar.SendMessageToAllAvatars("OnStartRecording");

    }


    [Command]
    public void CmdStopRecording()
    {
        if (!isRecording) return;

        Debug.Log("Stop Recording");
        isRecording = false;
        CommandLogger.Save();
        ViveAvatar.SendMessageToAllAvatars("OnStopRecording");
    }

    [Command]
    public void CmdStartReplay(string filename)
    {
        Debug.Log("Start Replay of " + filename);
        ReplayLog(CommandLogger.Load(filename));
    }

    [Command]
    public void CmdStartReplayFast(string filename)
    {
        Debug.Log("Start Replay of " + filename);
        ReplayLog(CommandLogger.Load(filename), false);
    }


    [Command]
    public void CmdStopReplay()
    {
        Debug.Log("StopReplay");
        stopReplay = true;
    }

    //------------------------


    [Server]
    public void ReplayLog(LogReader logReader, bool doRealTime = true)
    {
        if (isRecording || isReplay) return;

        StartCoroutine(DoReplayLog(logReader, doRealTime));
    }


    // Create an extra player object. Used for replay of recorded movements and commands.
    [Server]
    public GameObject SpawnExtraPlayer()
    {
		GameObject go = (GameObject)Instantiate(ViveNetworkManager.vnmSingleton.vivePlayerPrefab);
        NetworkServer.Spawn(go);
        go.GetComponent<ViveAvatar>().RpcSetGhost();
        go.GetComponent<ViveAvatar>().SpawnAvatar();
        return go;
    }

    private bool stopReplay;

    [Server]
	public IEnumerator DoReplayLog(LogReader logReader, bool doRealTime)
    {
        Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
        DateTime replayTimeStart = System.DateTime.Now;

        var bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

        ViveAvatar.SendMessageToAllAvatars("OnStartReplay");
        isReplay = true;

        stopReplay = false;

		foreach (LogEntry cmd in logReader)
        {
            if (stopReplay) break;

            if (!players.ContainsKey(cmd.networkId))
            {
                players[cmd.networkId] = SpawnExtraPlayer();
                if (!players[cmd.networkId].GetComponent<ViveAvatar>().initialized)
                    yield return new WaitForSeconds(0.01f);
            }

            DateTime when = replayTimeStart + (cmd.datetime - logReader.startTime);
            double delay = (when - System.DateTime.Now).TotalSeconds;
            if (doRealTime && (delay > 0)) yield return new WaitForSeconds((float)delay);

            if (cmd.manualParse)
			{
                players[cmd.networkId].SendMessage("OnParseLogCommand", cmd, SendMessageOptions.DontRequireReceiver);
                continue;
            }

            // This is a low-level hack. Unity replaces Commands functions with a Call prefix. I'm making use of that which might break in a later version.
            // Consider not allowing logging Commands or atleast warn about it in Debug Log.
            if (cmd.methodName.StartsWith("Cmd")) cmd.methodName = "Call" + cmd.methodName;
            try
            {
                Component c = players[cmd.networkId].GetComponent(cmd.className);
                c.GetType().GetMethod(cmd.methodName, bindingFlags).Invoke(c, cmd.parameters);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        foreach (GameObject player in players.Values)
        {
            player.GetComponent<ViveAvatar>().DestroyAvatar();
        }

        isReplay = false;
        ViveAvatar.SendMessageToAllAvatars("OnStopReplay");


    }

    //----
    // Client side listeners update the boolean flags in the clients
    // We couldn't use syncvars or Rpcs because each player has their own copy of this object...

    public void OnStartRecording() { isRecording = true; }
    public void OnStopRecording() { isRecording = false; }
    public void OnStopReplay() { isReplay = false; }
    public void OnStartReplay() { isReplay = true; }



}
