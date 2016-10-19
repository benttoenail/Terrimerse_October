using UnityEngine;
using System.Collections;

public class ToggleDataSet : MonoBehaviour {

	public GameObject subSetOne;
	public GameObject subSetTwo;
	public GameObject subSetThree;
	public GameObject subSetFour;
	public GameObject subSetFive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowSubSetOne(){
		print ("Showing Subset one");
		subSetOne.SetActive (true);
		subSetTwo.SetActive (false);
		subSetThree.SetActive (false);
		subSetFour.SetActive (false);
		subSetFive.SetActive (false);
	}

	public void ShowSubSetTwo(){
		print ("Showing Subset one");
		subSetOne.SetActive (false);
		subSetTwo.SetActive (true);
		subSetThree.SetActive (false);
		subSetFour.SetActive (false);
		subSetFive.SetActive (false);
	}

	public void ShowSubSetThree(){
		print ("Showing Subset one");
		subSetOne.SetActive (false);
		subSetTwo.SetActive (false);
		subSetThree.SetActive (true);
		subSetFour.SetActive (false);
		subSetFive.SetActive (false);
	}

	public void ShowSubSetFour(){
		print ("Showing Subset one");
		subSetOne.SetActive (false);
		subSetTwo.SetActive (false);
		subSetThree.SetActive (false);
		subSetFour.SetActive (true);
		subSetFive.SetActive (false);
	}

	public void ShowSubSetFive(){
		print ("Showing Subset one");
		subSetOne.SetActive (true);
		subSetTwo.SetActive (true);
		subSetThree.SetActive (true);
		subSetFour.SetActive (true);
		subSetFive.SetActive (true);
	}

}
