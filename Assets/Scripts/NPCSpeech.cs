using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchChatter;
using UnityEngine.UI;

public class NPCSpeech : MonoBehaviour {

	public TwitchManager manager;

	public TwitchText Speech;

	private GameObject child;

	void Awake(){
		child = transform.GetChild (0).gameObject;
		child.SetActive (false);
	}

	void OnTriggerEnter(Collider other){
		//enabke child
		child.SetActive(true);

		//get random string from the chatMessage list
		TwitchChatMessage x = manager.GetRandomMessage();

		//send it to canvas
		Speech.OnCustomMessage(ref x);
	}

	void OnTriggerExit(Collider other){
		//clear speech bubble
		Speech.Clear ();

		//disable child
		child.SetActive(false);
	}


}
