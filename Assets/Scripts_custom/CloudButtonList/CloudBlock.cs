using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudBlock : MonoBehaviour {

    TextMesh text;
    //List<string> cloudName = new List<string>();
    string cloudName;

    void OnEnable()
    {
        CloudButtonHandler.BlockCreated += InitBlock;
    }


    void InitBlock(string s)
    {
        cloudName = s;
        Debug.Log("Object created");
    }

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {
        text = GetComponent<TextMesh>();
        text.text = cloudName;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
