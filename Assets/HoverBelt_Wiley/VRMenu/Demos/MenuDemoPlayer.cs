using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.CompilerServices;

public class MenuDemoPlayer : NetworkBehaviour
{
	// Holds the viveAvatar object. Also used as a flag for when the Player is ready
	ViveAvatar viveAvatar = null;

	public GameObject interactorPrefab;

	private MenuDemoInteractor leftInteractor;
	private MenuDemoInteractor rightInteractor;

	// Called when the ViveAvatar is ready. Should be used instead of Start
	void OnViveReady(ViveAvatar _viveAvatar)
	{
		viveAvatar = _viveAvatar; // Save for later. Also usful as a flag for Update()

		// add menu interaction scripts if this vive is local to the client
		if (viveAvatar.isLocalVive)
		{
			GameObject leftInteractorObj = (GameObject)Instantiate (interactorPrefab);
			leftInteractorObj.transform.SetParent (viveAvatar.controllers [ViveAvatar.LEFT].transform, false);
			leftInteractor = leftInteractorObj.GetComponent<MenuDemoInteractor> ();
			leftInteractor.Configure (this,viveAvatar.controllers [ViveAvatar.LEFT]);


			GameObject rightInteractorObj = (GameObject)Instantiate (interactorPrefab);
			rightInteractorObj.transform.SetParent (viveAvatar.controllers [ViveAvatar.RIGHT].transform, false);
			rightInteractor = rightInteractorObj.GetComponent<MenuDemoInteractor> ();
			rightInteractor.Configure (this,viveAvatar.controllers [ViveAvatar.RIGHT]);

		}

	}

	// Update is called once per frame
	void Update()
	{
		if (!viveAvatar || !viveAvatar.isLocalVive) return;

		if (leftInteractor.isIntersecting) {
			leftInteractor.HandleInput ();
		} else {
			// Tool-specific behavior
		}
		if (rightInteractor.isIntersecting) {
			rightInteractor.HandleInput ();
		} else {
			// Tool-specific behavior
		}

	}

}