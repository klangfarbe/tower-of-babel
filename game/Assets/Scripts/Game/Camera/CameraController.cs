using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameController gameController;
	private GameObject gameCamera;
	private GameObject levelCenter;
	private LevelInfo levelInfo;

	private float distanceToSpider = 1.25f;
	private float distanceToLevel = 10f;

	private float cameraHeightToSpider = 0.3f;
	private float cameraHeightToLevel = 0.3f;

	private float cameraAngleToSpider = 0.5f;
	private float cameraAngleToLevel = 3f;

	private float minCameraAngle = 3f;
	private float maxCameraAngle = 20f;

	private float maxZoomIn = 1.5f;
	private float maxZoomOut = 10f;

	private float cameraLightIntensity;

	public bool mapActive = false;

	// ------------------------------------------------------------------------

	void Awake() {
		gameCamera = GameObject.Find("GameCam");
		levelCenter = GameObject.Find("Level/LevelCenter");
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
		levelInfo = GameObject.Find("Level").GetComponent<LevelInfo>();
		cameraLightIntensity = gameCamera.camera.GetComponent<Light>().intensity;
	}

	// ------------------------------------------------------------------------

	public void init(GameObject obj) {
		gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
		lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider);
		distanceToLevel = maxZoomIn * 2.5f;
	}

	// ------------------------------------------------------------------------

	public bool lookAt(GameObject obj, float distance, float height, float angle) {
		if(!obj)
			return false;
		gameController.activeObject = obj;
		gameCamera.GetComponent<FollowingCamera>().set(obj, distance, height, angle);
		return true;
	}

	// ------------------------------------------------------------------------

	public void activateGrabber() {
		Debug.Log("Activating Grabber");
		GameObject obj = GameObject.Find("GRB");
		if(lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider)) {
			gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
			mapActive = false;
		}
	}

	// ------------------------------------------------------------------------

	public void activatePusher() {
		Debug.Log("Activating Pusher");
		GameObject obj = GameObject.Find("PSH");
		if(lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider)) {
			gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
			mapActive = false;
		}
	}

	// ------------------------------------------------------------------------

	public void activateZapper() {
		Debug.Log("Activating Zapper");
		GameObject obj = GameObject.Find("ZAP");
		if(lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider)) {
			gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
			mapActive = false;
		}
	}

	// ------------------------------------------------------------------------

	public void activateOverview() {
		if(levelInfo.cameras) {
			if(lookAt(levelCenter, distanceToLevel, cameraHeightToLevel, cameraAngleToLevel)) {
				gameCamera.GetComponent<Light>().intensity = cameraLightIntensity * 0.5f * distanceToLevel;
				mapActive = true;
				zoom(0);
			}
		}
	}

	// ------------------------------------------------------------------------

	public void setFieldOfView(int angle) {
		gameCamera.GetComponent<Camera>().fieldOfView = angle;
	}

	// ------------------------------------------------------------------------

	public void zoom(float zoom) {
		if(!mapActive)
			return;
		distanceToLevel += zoom;
		distanceToLevel = Mathf.Clamp(distanceToLevel, maxZoomIn, maxZoomOut);

		// calculate the necessary distance to the level center
		float levelBoundsDistanceToLevelCenter = 0f;

		RaycastHit hit;
		Debug.DrawRay (levelCenter.transform.position -levelCenter.transform.forward * 5, levelCenter.transform.forward * 5, Color.blue, 5f);

		if(Physics.Raycast(levelCenter.transform.position -levelCenter.transform.forward * 20, levelCenter.transform.forward, out hit, 1 << 8)) {
			levelBoundsDistanceToLevelCenter = Vector3.Distance(hit.point, levelCenter.transform.position);
		}

		gameCamera.GetComponent<FollowingCamera>().Distance = distanceToLevel + levelBoundsDistanceToLevelCenter;
		gameCamera.GetComponent<Light>().intensity = cameraLightIntensity * 0.5f * distanceToLevel;
	}

	// ------------------------------------------------------------------------

	public void translateOverview(Vector2 v) {
		if(!mapActive)
			return;

		var l = levelCenter.transform.position;
		Vector3 v2 = Vector3.zero;

		// make sure the camera could not move outside the playing area
		// depending on the rotation we must create a translation vector
		// which will move the levelcenter around only inside the bounding
		// box of the level
		v2.y = calculateMaxTranslation(levelInfo.maxFloors + 0.5f, l.y, v.y);

		int angle = (int)levelCenter.transform.localEulerAngles.y;
		if(angle == 0) {
			v2.x = calculateMaxTranslation(levelInfo.maxColumns, l.x, v.x);
		} else if(angle == 90) {
			v2.z = calculateMaxTranslation(levelInfo.maxRows, l.z, -v.x);
		} else if(angle == 180) {
			v2.x = calculateMaxTranslation(levelInfo.maxColumns, l.x, -v.x);
		} else if(angle == 270) {
			v2.z = calculateMaxTranslation(levelInfo.maxRows, l.z, v.x);
		}
		levelCenter.transform.Translate(v2, Space.World);
	}

	// ------------------------------------------------------------------------

	private float calculateMaxTranslation(float max, float x, float y) {
		if(x + y < 0f) {
			return -x;
		}
		if(x + y >= max) {
			return max - x;
		}
		return y;
	}

	// ------------------------------------------------------------------------

	public void rotateOverview(float angle) {
		if(!mapActive)
			return;
		levelCenter.transform.RotateAround(levelCenter.transform.position, Vector3.up, angle);
		gameCamera.GetComponent<FollowingCamera>().startTime = 0;
		zoom(0);
	}

	// ------------------------------------------------------------------------

	public void changeCameraAngle(float angle) {
		if(!mapActive)
			return;
		cameraAngleToLevel += angle;
		cameraAngleToLevel = Mathf.Clamp(cameraAngleToLevel, minCameraAngle, maxCameraAngle);
		gameCamera.GetComponent<FollowingCamera>().Angle = cameraAngleToLevel;
	}
}
