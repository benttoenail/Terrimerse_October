using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;

public class FightPlayer : NetworkBehaviour
{
	// Holds the viveAvatar object. Also used as a flag for when the Player is ready
	ViveAvatar viveAvatar = null;

	public GameObject handAddonsL, handAddonsR; // the hand addons hold the the saber gun and hands meshes
	public GameObject head;
	public GameObject healthBar;
	public enum State
	{
		Saber, Gun
	}

	[SyncVar]
	public State leftState, rightState; // The state of each hand

	[Command]
	void CmdSetState(int controllerIndex, int state)
	{
		CommandLogger.Log(this, controllerIndex, state);

		if (controllerIndex == ViveAvatar.LEFT) leftState = (State)state;
		if (controllerIndex == ViveAvatar.RIGHT) rightState = (State)state;
	}

	// Called when the ViveAvatar is ready. Should be used instead of Start
	void OnViveReady(ViveAvatar _viveAvatar)
	{
		viveAvatar = _viveAvatar; // Save for later. Also usful as a flag for Update()

		// Setup the hands head and health score
		handAddonsL = (GameObject)Instantiate(Resources.Load<GameObject>("HandAddons"));
		handAddonsL.transform.SetParent(viveAvatar.controllers[ViveAvatar.LEFT].transform, false);
		handAddonsL.transform.Find("RightHand").gameObject.SetActive(false);
		handAddonsR = (GameObject)Instantiate(Resources.Load<GameObject>("HandAddons"));
		handAddonsR.transform.SetParent(viveAvatar.controllers[ViveAvatar.RIGHT].transform, false);
		handAddonsR.transform.Find("LeftHand").gameObject.SetActive(false);
		head = (GameObject)Instantiate(Resources.Load<GameObject>("head"));
		head.transform.SetParent(viveAvatar.controllers[ViveAvatar.HEAD].transform, false);
		healthBar = (GameObject)Instantiate(Resources.Load<GameObject>("HealthBar"));
		healthBar.transform.SetParent(viveAvatar.controllers[ViveAvatar.HEAD].transform, false);

		// initial states
		leftState = State.Saber;
		rightState = State.Gun;

		// remove the models of the controllers if this is a local vive
		if (viveAvatar.isLocalVive)
		{
			viveAvatar.controllers[ViveAvatar.LEFT].model.SetActive(false);
			viveAvatar.controllers[ViveAvatar.RIGHT].model.SetActive(false);
		}

	}

	// Update is called once per frame
	void Update()
	{
		if (!viveAvatar) return;

		// Update the visibility of the weapons based on the their state
		handAddonsL.transform.Find("Pistol").gameObject.SetActive(leftState == State.Gun);
		handAddonsL.transform.Find("LightSaber").gameObject.SetActive(leftState == State.Saber);
		handAddonsR.transform.Find("Pistol").gameObject.SetActive(rightState == State.Gun);
		handAddonsR.transform.Find("LightSaber").gameObject.SetActive(rightState == State.Saber);

		// if this is the local vive, look at controls and decide what to do
		if (viveAvatar.isLocalVive)
		{
			// Fire if trigger and gun is selected
			if (leftState == State.Gun && viveAvatar.controllers[ViveAvatar.LEFT].isTriggerClick)
			{
				CmdFire(ViveAvatar.LEFT);
			}
			if (rightState == State.Gun && viveAvatar.controllers[ViveAvatar.RIGHT].isTriggerClick)
			{
				CmdFire(ViveAvatar.RIGHT);
			}

			// If grip is touched start/stop recording.
			if (viveAvatar.controllers[ViveAvatar.LEFT].isGripClick)
			{
				if (!GetComponent<ViveNetworkFunctions>().isRecording)
				{
					GetComponent<ViveNetworkFunctions>().CmdStartRecording("TempRecording.vnmpb");
				} else
				{
					GetComponent<ViveNetworkFunctions>().CmdStopRecording();
				}
			}
			if (viveAvatar.controllers[ViveAvatar.RIGHT].isGripClick)
			{
				if (!GetComponent<ViveNetworkFunctions>().isReplay)
				{
					GetComponent<ViveNetworkFunctions>().CmdStartReplay("TempRecording.vnmpb");
				}
				else
				{
					GetComponent<ViveNetworkFunctions>().CmdStopReplay();
				}
			}

			// if TouchPad is pressed, toggle weapon
			if (viveAvatar.controllers[ViveAvatar.LEFT].isTouchPadClick) CmdSetState(ViveAvatar.LEFT, (int)leftState.Next());
			if (viveAvatar.controllers[ViveAvatar.RIGHT].isTouchPadClick) CmdSetState(ViveAvatar.RIGHT, (int)rightState.Next());
		}
	}

	[Command]
	void CmdFire(int controllerIndex)
	{
		CommandLogger.Log(this, controllerIndex);

		Transform t = viveAvatar.controllers[controllerIndex].transform;

		// create the bullet object from the bullet prefab
		var bullet = (GameObject)Instantiate(
			Resources.Load("Bullet"),
			t.position - t.up * 0.1f + t.forward * 0.03f,
			Quaternion.identity);

		// make the bullet move away in front of the player
		bullet.GetComponent<Rigidbody>().velocity = -t.up * 1;

		// spawn the bullet on the clients
		NetworkServer.Spawn(bullet);

		// make bullet disappear after 2 seconds
		Destroy(bullet, 2.0f);

	}

	public void OnStartRecording()
	{
		// When recording starts, we want to send mock commands so they are logged and the inital state of the object is saved.
		CmdSetState(ViveAvatar.LEFT, (int)leftState);
		CmdSetState(ViveAvatar.RIGHT, (int)rightState);
	}

}