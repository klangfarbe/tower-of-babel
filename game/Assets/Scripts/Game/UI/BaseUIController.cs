using UnityEngine;
using System.Collections;

public class BaseUIController : MonoBehaviour {
	protected CameraController cameraController;
	protected GameController gameController;
	protected Conditions conditions;

	protected float sWidth = 1024f;
	protected float sHeight = 768f;

	// ------------------------------------------------------------------------

	void Start() {
		cameraController = GameObject.Find("Controller").GetComponent<CameraController>();
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
		conditions = GameObject.Find("Level").GetComponent<Conditions>();
	}

	// ------------------------------------------------------------------------

	protected void initGuiScale() {
		calculateAspectRatio();

		float xFactor = Screen.width / sWidth;
		float yFactor = Screen.height / sHeight;
		GUIUtility.ScaleAroundPivot(new Vector2(xFactor, yFactor), Vector2.zero);
	}

	// ------------------------------------------------------------------------

	protected void calculateAspectRatio () {
		var aspect = (float)Screen.width / (float)Screen.height;
		if(aspect >= 1.7f && aspect < 1.78f) { // 16:9
			sWidth = 1280;
			sHeight = 720;
		} else if(aspect >= 1.25f && aspect < 1.35f) { // 4:3
			sWidth = 1024;
			sHeight = 768;
		} else if(aspect >= 1.55f && aspect < 1.65f) { // 16:10
			sWidth = 1280;
			sHeight = 800;
		} else if(aspect >= 1.45f && aspect < 1.53f) { // 3:2
			sWidth = 1280;
			sHeight = 854;
		}
		//Debug.Log(aspect + " / " + Screen.width + " / " + Screen.height);
	}
}