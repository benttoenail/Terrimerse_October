using UnityEngine;
using System.Collections;

public class HoverBeltMotion : MonoBehaviour {

    public GameObject headSet;
    public GameObject playerGroup;
    bool parented = false;

    public float scaleFix;

	HoverBeltItems control;

	public void Configure(GameObject _headSet, GameObject _playerGroup) { // Add Parent object upon instatiation
		headSet = _headSet;
        playerGroup = _playerGroup;
	}

    void Start()
    {
        control = GetComponentInChildren<HoverBeltItems>();
        //gameObject.transform.localScale = new Vector3(scaleFix, scaleFix, scaleFix);
       
    }
    void Update () {
        if (!parented)
        {
            ParentToPlayer();
        }

		if (headSet == null)
			return;
		
		if (control.state == HoverBeltItems.BeltState.Closed) {
			float increment = Time.deltaTime * control.moveSpeed;
			//transform.position = Vector3.Lerp (transform.position, headSet.transform.position + control.baseHeight * Vector3.up, increment);
            transform.position = Vector3.Lerp(transform.position, headSet.transform.position, increment);
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (headSet.transform.eulerAngles.y, Vector3.up), increment);
		}
	}

    void ParentToPlayer()
    {
        gameObject.transform.SetParent(playerGroup.transform);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        parented = true;
    }
}
