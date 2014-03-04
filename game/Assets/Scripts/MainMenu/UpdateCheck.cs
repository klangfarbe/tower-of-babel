using UnityEngine;
using System.Collections;
using System;

public class UpdateCheck : MonoBehaviour {
	private GUINotification gui;

	// ------------------------------------------------------------------------

	void Start() {
		gui = gameObject.GetComponent<GUINotification>();
	}

	// ------------------------------------------------------------------------

	public IEnumerator checkForUpdate() {
		Debug.Log("Checking for update");
		gui.notify("Checking for update", 2);
		WWW www = new WWW("http://tob.guzumi.de/version.txt");
		yield return www;
		Debug.Log(www.text + " / <" + www.error + "> / " + ((TextAsset)Resources.Load("version")).text);
		if(www.text != ((TextAsset)Resources.Load("version")).text) {
			gui.notify("Version " + www.text + " is available\nYou can download it at http://tob.guzumi.de", 5);
		}
	}
}