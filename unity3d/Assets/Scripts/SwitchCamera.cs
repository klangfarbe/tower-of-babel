using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	GameObject[] camList;
	GameObject mainCamera;
	
	void Start() {
		mainCamera = GameObject.Find("MainCamera");
	}
	
	// ------------------------------------------------------------------------
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			activateCamera("MainCamera");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			activateCamera("camera_grabber");
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			activateCamera("camera_zapper");
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			activateCamera("camera_pusher");
		}
	}
	
	// ------------------------------------------------------------------------
	
	public void reset() {
		Debug.Log("Resetting Camera List");
		camList = GameObject.FindGameObjectsWithTag("MainCamera");
		System.Array.Resize(ref camList, camList.Length + 1);
		camList[camList.Length - 1] = mainCamera;
			//(mainCamera, camList.Length);
		activateCamera("MainCamera");
	}
	
	// ------------------------------------------------------------------------
	
	void activateCamera(string cam) {
		Debug.Log(cam);
		 
		foreach(GameObject c in camList) {
			Debug.Log (c.name);
			if(cam == c.name) {
				c.SetActive(true);
			} else {
				c.SetActive(false);
			}
		}
	}
}
