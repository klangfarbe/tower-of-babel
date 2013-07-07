using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	GameObject[] camList;

	// -----------------------------------------------------------------------------------------------------------------

	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			activateCamera("MainCamera");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			activateCamera("GrabberCamera");
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			activateCamera("ZapperCamera");
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			activateCamera("PusherZamera");
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void reset() {
		Debug.Log("Resetting Camera List");
		camList = GameObject.FindGameObjectsWithTag("MainCamera");
		assignToCamera(GameObject.Find("GrabberCamera"), GameObject.Find("GRB"));
		assignToCamera(GameObject.Find("PusherCamera"), GameObject.Find("PSH"));
		assignToCamera(GameObject.Find("ZapperCamera"), GameObject.Find("ZAP"));
		activateCamera("MainCamera");
	}

	// -----------------------------------------------------------------------------------------------------------------

	void assignToCamera(GameObject cam, GameObject spider) {
		if(cam) {
			SmoothFollow s = cam.GetComponent<SmoothFollow>();
			if(spider) {
				s.target = spider.transform;
			}
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

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
