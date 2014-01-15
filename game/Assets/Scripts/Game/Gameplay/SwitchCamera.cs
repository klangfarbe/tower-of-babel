using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	public GameObject mainCamera;
	public GameObject spiderCamera;

	// ------------------------------------------------------------------------

	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			mainCamera.SetActive(true);
			spiderCamera.SetActive(false);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			activateGrabber();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			activatePusher();
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			activateZapper();
		}

		if(!spiderCamera.GetComponent<SpiderCamera>().target) {
			if(GameObject.Find("GRB")) {
				activateGrabber();
			} else if(GameObject.Find("PSH")) {
				activatePusher();
			} else if(GameObject.Find("ZAP")) {
				activateZapper();
			}
		}
	}

	// ------------------------------------------------------------------------

	void activateSpiderCamera(GameObject spider) {
		Debug.Log(spider);
		spiderCamera.GetComponent<SpiderCamera>().target = spider;
		mainCamera.SetActive(false);
		spiderCamera.SetActive(true);
	}

	// ------------------------------------------------------------------------

	public void activateGrabber() {
		Debug.Log("Activating Grabber");
		activateSpiderCamera(GameObject.Find("GRB"));
	}

	// ------------------------------------------------------------------------

	public void activatePusher() {
		Debug.Log("Activating Pusher");
		activateSpiderCamera(GameObject.Find("PSH"));
	}

	// ------------------------------------------------------------------------

	public void activateZapper() {
		Debug.Log("Activating Zapper");
		activateSpiderCamera(GameObject.Find("ZAP"));
	}

	// ------------------------------------------------------------------------

	public void activateOverview() {
		mainCamera.SetActive(true);
		spiderCamera.SetActive(false);
	}

	// ------------------------------------------------------------------------

	void setFieldOfView(int angle) {
		spiderCamera.GetComponent<Camera>().fieldOfView = angle;
	}
}
