using UnityEngine;
using System.Collections;

public class ScaleDataSet : MonoBehaviour {

    Vector3 scale;

	// Use this for initialization
	void Start () {

        scale = this.gameObject.transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetScaleOne()
    {
        Debug.Log("Button One Pressed!!");
        this.gameObject.transform.localScale = new Vector3(scale.x * 0.7f, scale.y * 0.7f, scale.z * 0.7f);

    }

    public void SetScaleTwo()
    {
        Debug.Log("Button Two Pressed!!");
        this.gameObject.transform.localScale = scale;
    }

    public void SetScaleThree()
    {
        Debug.Log("Button Three Pressed");
        this.gameObject.transform.localScale = new Vector3(scale.x * 1.7f, scale.y * 1.7f, scale.z * 1.7f);
    }
}
