using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	public GameObject mainCamera;
	public GameObject spiderCamera;

	// ------------------------------------------------------------------------

	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			activateGrabber();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			activatePusher();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			activateZapper();
		}
		if(Input.GetKeyDown(KeyCode.M) && GameObject.Find("Level").GetComponent<Behaviour>().cameras) {
			mainCamera.SetActive(true);
			spiderCamera.SetActive(false);
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine(GameObject.Find("Level").GetComponent<Conditions>().levelFailed());
		}
	}

	// ------------------------------------------------------------------------

	public void activateSpiderCamera(GameObject spider) {
		if(!spider)
			return;
		spiderCamera.GetComponent<SpiderCamera>().Target = spider;
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