using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameController gameController;
	private GameObject gameCamera;
	private GameObject levelCenter;
	private LevelLoader level;

	private float distanceToSpider = 1.25f;
	private float distanceToLevel = 10f;

	private float cameraHeightToSpider = 0.3f;
	private float cameraHeightToLevel = 0.3f;

	private float cameraAngleToSpider = 0.5f;
	private float cameraAngleToLevel = 3f;

	private float minCameraAngle = 2f;
	private float maxCameraAngle = 40f;

	private float maxZoomIn = 1.5f;
	private float maxZoomOut = 10f;

	private float cameraLightIntensity;

	public bool mapActive = false;

	// ------------------------------------------------------------------------

	void Awake() {
		gameCamera = GameObject.Find("GameCam");
		levelCenter = GameObject.Find("Level/LevelCenter");
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
		level = GameObject.Find("Level").GetComponent<LevelLoader>();
		cameraLightIntensity = gameCamera.GetComponent<Camera>().GetComponent<Light>().intensity;
	}

	// ------------------------------------------------------------------------

	public void init() {
		gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
		distanceToLevel = 8f;
		cameraHeightToLevel = 0.8f;
		cameraAngleToLevel = 10f;
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

	public void activateGrabber(GameObject obj = null) {
		if(!obj) obj = GameObject.Find("GRB");
		if(lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider)) {
			gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
			gameCamera.GetComponent<FollowingCamera>().Force = true;
			mapActive = false;
			Debug.Log("Activating Grabber");
		}
	}

	// ------------------------------------------------------------------------

	public void activatePusher(GameObject obj = null) {
		if(!obj) obj = GameObject.Find("PSH");
		if(lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider)) {
			gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
			gameCamera.GetComponent<FollowingCamera>().Force = true;
			mapActive = false;
			Debug.Log("Activating Pusher");
		}
	}

	// ------------------------------------------------------------------------

	public void activateZapper(GameObject obj = null) {
		if(!obj) obj = GameObject.Find("ZAP");
		if(lookAt(obj, distanceToSpider, cameraHeightToSpider, cameraAngleToSpider)) {
			gameCamera.GetComponent<Light>().intensity = cameraLightIntensity;
			gameCamera.GetComponent<FollowingCamera>().Force = true;
			mapActive = false;
			Debug.Log("Activating Zapper");
		}
	}

	// ------------------------------------------------------------------------

	public void activateOverview() {
		if(level.Cameras) {
			if(lookAt(levelCenter, distanceToLevel, cameraHeightToLevel, cameraAngleToLevel)) {
				gameCamera.GetComponent<Light>().intensity = cameraLightIntensity * 0.5f * distanceToLevel;
				gameCamera.GetComponent<FollowingCamera>().Force = false;
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
		v2.y = calculateMaxTranslation(level.MaxFloors + 0.5f, l.y, v.y);

		int angle = (int)levelCenter.transform.localEulerAngles.y;
		if(angle == 0) {
			v2.x = calculateMaxTranslation(level.MaxColumns, l.x, v.x);
		} else if(angle == 90) {
			v2.z = calculateMaxTranslation(level.MaxRows, l.z, -v.x);
		} else if(angle == 180) {
			v2.x = calculateMaxTranslation(level.MaxColumns, l.x, -v.x);
		} else if(angle == 270) {
			v2.z = calculateMaxTranslation(level.MaxRows, l.z, v.x);
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
