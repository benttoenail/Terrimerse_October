using UnityEngine;
using System.Collections;

public class CloudButtonHandler : MonoBehaviour {

    [SerializeField] GameObject DataSet;
    [SerializeField]
    GameObject blockPrefab;

	// Use this for initialization
	void Start () {

        float y = 0;
        Vector3 handlerPos = transform.position;
        foreach(Transform c in DataSet.transform)
        {
            Debug.Log("here you go..." + c.transform.gameObject.name);
            GameObject block = Instantiate(blockPrefab, new Vector3(handlerPos.x, handlerPos.y + y, handlerPos.z), Quaternion.identity) as GameObject;
            block.transform.SetParent(gameObject.transform);
            y = y - 50;
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
