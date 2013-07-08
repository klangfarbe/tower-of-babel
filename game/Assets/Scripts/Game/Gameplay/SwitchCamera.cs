using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	public GameObject mainCamera;
	public GameObject spiderCamera;

	// -----------------------------------------------------------------------------------------------------------------

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
	}

	// -----------------------------------------------------------------------------------------------------------------

	void activateSpiderCamera(GameObject spider) {
		SpiderCamera s = spiderCamera.GetComponent<SpiderCamera>();
		if(spider != null) {
			s.target = spider.transform;
			setFieldOfView(35);
		} else {
			s.target = GameObject.Find("DisabledCamera").transform;
			setFieldOfView(70);
		}
		mainCamera.SetActive(false);
		spiderCamera.SetActive(true);
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateGrabber() {
		activateSpiderCamera(GameObject.Find("LookAtGrabber"));
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activatePusher() {
		activateSpiderCamera(GameObject.Find("LookAtPusher"));
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateZapper() {
		activateSpiderCamera(GameObject.Find("LookAtZapper"));
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateOverview() {
		mainCamera.SetActive(true);
		spiderCamera.SetActive(false);
	}

	// -----------------------------------------------------------------------------------------------------------------

	void setFieldOfView(int angle) {
		spiderCamera.GetComponent<Camera>().fieldOfView = angle;
	}
}
