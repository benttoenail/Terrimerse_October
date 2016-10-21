using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CloudButtonHandler : MonoBehaviour {

    [SerializeField] GameObject DataSet;
    [SerializeField]
    GameObject blockPrefab;

    //string[] names;
    List<string> names = new List<string>();
    GameObject[] objects;

    public delegate void CloudListEventHandler(string s);
    public static event CloudListEventHandler BlockCreated;

	// Use this for initialization
	void Start () {

        float y = 0;
        Vector3 handlerPos = transform.position;

        foreach(Transform c in DataSet.transform)
        {
            GameObject block = Instantiate(blockPrefab, new Vector3(handlerPos.x, handlerPos.y + y, handlerPos.z), Quaternion.identity) as GameObject;
            BlockCreated(c.gameObject.name);
            block.transform.SetParent(gameObject.transform);
            Debug.Log(c.gameObject.name); // This is Correct!

            names.Add(c.gameObject.name);
            y = y - 50;
        }

        /*
        for(int i = 0; i < names.Count; i++)
        {
            BlockCreated(names[i]);
        }
        */

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
