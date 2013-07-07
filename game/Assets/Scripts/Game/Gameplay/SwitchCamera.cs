using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	public GameObject mainCamera;
	public GameObject spiderCamera;

	void Start() {
		mainCamera.SetActive(true);
		spiderCamera.SetActive(false);
	}

	// -----------------------------------------------------------------------------------------------------------------

	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			mainCamera.SetActive(true);
			spiderCamera.SetActive(false);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			activateSpiderCamera(GameObject.Find("LookAtGrabber"));
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			activateSpiderCamera(GameObject.Find("LookAtPusher"));
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			activateSpiderCamera(GameObject.Find("LookAtZapper"));
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	void activateSpiderCamera(GameObject spider) {
		SpiderCamera s = spiderCamera.GetComponent<SpiderCamera>();
		if(spider != null) {
			s.target = spider.transform;
		} else {
			s.target = GameObject.Find("DisabledCamera").transform;
		}
		mainCamera.SetActive(false);
		spiderCamera.SetActive(true);
	}
}
