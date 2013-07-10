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
			s.target = spider;
			setFieldOfView(35);
		} else {
			s.target = GameObject.Find("DisabledCamera");
			setFieldOfView(70);
		}
		mainCamera.SetActive(false);
		spiderCamera.SetActive(true);
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateGrabber() {
		activateSpiderCamera(GameObject.Find("GRB"));
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activatePusher() {
		activateSpiderCamera(GameObject.Find("PSH"));
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateZapper() {
		activateSpiderCamera(GameObject.Find("ZAP"));
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
