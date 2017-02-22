using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControllers : MonoBehaviour {

	public GameObject secondaryMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Tab)) {
			//open the inventory screen and map
			if (secondaryMenu.activeSelf) {
				secondaryMenu.SetActive (false);
			} else {
				secondaryMenu.SetActive (true);
			}
		}


	}
}
