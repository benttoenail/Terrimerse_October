using UnityEngine;
using System.Collections;

public class RadialGridFollowDataSet : MonoBehaviour {

    int count;
    public float speed;

    Transform[] circle;
    public GameObject dataSet;

	// Use this for initialization
	void Start () {

        count = transform.childCount;

        //Put all circles into array - from Big to small
        circle = new Transform[transform.childCount];
        for(int i = 0; i < count; i++)
        {
            circle[i] = transform.GetChild(i);
        }

	}
	
	// Update is called once per frame
	void Update () {

        //Move.Lerp the Circles to DataSet
        for (int i = 0; i < count; i++)
        {
            circle[i].transform.position = Vector3.Lerp(circle[i].transform.position, dataSet.transform.position / (i+1.2f), speed * Time.deltaTime);
        }


    }
}
