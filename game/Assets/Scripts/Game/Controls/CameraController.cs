using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject camera;

	private float distanceToSpider = 1.25f;
	private float distanceToLevel = 10f;

	private float cameraHeightToSpider = 0.3f;
	private float cameraHeightToLevel = 1;

	private float cameraAngleToSpider = 0.1f;
	private float cameraAngleToLevel = 0.1f;

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
		if(Input.GetKeyDown(KeyCode.M)) {
			activateOverview();
		}
		if(Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine(GameObject.Find("Level").GetComponent<Conditions>().levelFailed());
		}
	}

	// ------------------------------------------------------------------------

	public void init(GameObject obj) {
		lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider);
	}

	// ------------------------------------------------------------------------

	public void lookAt(GameObject obj, float distance, float height, float angle) {
		if(!obj)
			return;
		camera.GetComponent<SpiderCamera>().set(obj, distance, height, angle);
	}

	// ------------------------------------------------------------------------

	public void activateGrabber() {
		Debug.Log("Activating Grabber");
		GameObject obj = GameObject.Find("GRB");
		lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider);
	}

	// ------------------------------------------------------------------------

	public void activatePusher() {
		Debug.Log("Activating Pusher");
		GameObject obj = GameObject.Find("PSH");
		lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider);
	}

	// ------------------------------------------------------------------------

	public void activateZapper() {
		Debug.Log("Activating Zapper");
		GameObject obj = GameObject.Find("ZAP");
		lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider);
	}

	// ------------------------------------------------------------------------

	public void activateOverview() {
		if(GameObject.Find("Level").GetComponent<Behaviour>().cameras)
			lookAt(GameObject.Find("Level/LevelCenter"), distanceToLevel, cameraHeightToLevel, cameraAngleToLevel);
	}

	// ------------------------------------------------------------------------

	void setFieldOfView(int angle) {
		camera.GetComponent<Camera>().fieldOfView = angle;
	}
}
