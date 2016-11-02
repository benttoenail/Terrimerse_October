using UnityEngine;
using System.Collections;

public class SpriteScale : MonoBehaviour {

    GameObject player;
    Bounds bounds;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        bounds = GetComponent<SpriteRenderer>().sprite.bounds;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = player.transform.localScale / 10;
	}
}
