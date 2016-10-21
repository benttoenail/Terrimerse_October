using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CloudButtonHandler : MonoBehaviour {

    [SerializeField] GameObject DataSet;
    [SerializeField]
    GameObject blockPrefab;

    GameObject[] objects;

    //Leave these for now - they may come in handy later...
    public delegate void CloudListEventHandler(string s);
    public static event CloudListEventHandler BlockCreated;

    GameObject PCListTransform;

    void Awake()
    {
        PCListTransform = new GameObject();
        PCListTransform.transform.position = transform.position;
        PCListTransform.name = "PCListTransform";
        PCListTransform.transform.SetParent(gameObject.transform);
    }

	// Use this for initialization
	void Start () {

        float y = 0;
        float spacing = 0;
        Vector3 handlerPos = transform.position;


        foreach(Transform c in DataSet.transform)
        {
            GameObject block = Instantiate(blockPrefab, new Vector3(handlerPos.x, handlerPos.y + y + spacing, handlerPos.z), Quaternion.identity) as GameObject;
            
            block.transform.SetParent(PCListTransform.gameObject.transform);

            block.GetComponent<CloudBlock>().GetCloudData(c.gameObject.name, c.gameObject);

            y = y - 50;
            spacing = spacing - 2;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
