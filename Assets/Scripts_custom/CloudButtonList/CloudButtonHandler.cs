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

    //Leave these for now - they may come in handy later...
    public delegate void CloudListEventHandler(string s);
    public static event CloudListEventHandler BlockCreated;



	// Use this for initialization
	void Start () {

  

        float y = 0;
        float spacing = 0;
        Vector3 handlerPos = transform.position;

        foreach(Transform c in DataSet.transform)
        {
            GameObject block = Instantiate(blockPrefab, new Vector3(handlerPos.x, handlerPos.y + y + spacing, handlerPos.z), Quaternion.identity) as GameObject;
            
            block.transform.SetParent(gameObject.transform);
            block.GetComponent<CloudBlock>().GetCloudData(c.gameObject.name, c.gameObject);
            y = y - 50;
            spacing = spacing - 2;
        }


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
