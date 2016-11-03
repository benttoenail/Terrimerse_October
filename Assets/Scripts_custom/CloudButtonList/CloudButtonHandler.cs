using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CloudButtonHandler : MonoBehaviour {

    // [SerializeField] GameObject DataSet;
    [SerializeField]
    Transform DataSetMeshes;

    [SerializeField]
    Transform DataSetPointClouds;

    [SerializeField]
    GameObject blockPrefab;

    GameObject DataSet;

    //Leave these for now - they may come in handy later...
    public delegate void CloudListEventHandler(string s);
    public static event CloudListEventHandler BlockCreated;

    GameObject PCListTransform;
    int count;

    void Awake()
    {
        PCListTransform = new GameObject();
        PCListTransform.transform.position = transform.position;
        PCListTransform.name = "PCListTransform";
        PCListTransform.transform.SetParent(gameObject.transform);
        
    }

	// Use this for initialization
	//void Start () {
    public void InitDataSet(Quaternion _rotation) { 
        DataSet = GameObject.FindGameObjectWithTag("DataSet");

        DataSetMeshes = DataSet.gameObject.transform.Find("DataSet 17 - 41/MeshedCloud_dataSet");
        DataSetPointClouds = DataSet.gameObject.transform.Find("DataSet 17 - 41/PointCloud_dataSet");

        float y = 50;
        float spacing = 0;
        count = DataSetPointClouds.transform.childCount;
        Vector3 handlerPos = transform.position;

        GameObject[] pointClouds = new GameObject[count];
        GameObject[] meshClouds = new GameObject[count];


        for(int i = 0; i < count; i++)
        {
            pointClouds[i] = DataSetPointClouds.transform.GetChild(i).gameObject;
            meshClouds[i] = DataSetMeshes.transform.GetChild(i).gameObject;

            GameObject block = Instantiate(blockPrefab, new Vector3(handlerPos.x, handlerPos.y + y + spacing, handlerPos.z), Quaternion.identity) as GameObject;
            block.transform.SetParent(PCListTransform.gameObject.transform);
            block.GetComponent<CloudBlock>().GetCloudData(pointClouds[i].gameObject.name, pointClouds[i].gameObject, meshClouds[i].gameObject, pointClouds[i].activeSelf);

            y = y - 50;
            spacing = spacing - 2;
            
        }

        PCListTransform.transform.rotation = _rotation;
    }
	
}
