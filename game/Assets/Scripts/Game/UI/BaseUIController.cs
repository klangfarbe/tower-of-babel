using UnityEngine;
using System.Collections;

public class BaseUIController : MonoBehaviour {
	protected CameraController cameraController;
	protected GameController gameController;
	protected Conditions conditions;
	protected LevelInfo levelInfo;

	protected AspectRatio ar;

	// ------------------------------------------------------------------------

	void Start() {
		ar = new AspectRatio();

		cameraController = GameObject.Find("Controller").GetComponent<CameraController>();
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
		conditions = GameObject.Find("Level").GetComponent<Conditions>();
		levelInfo = GameObject.Find("Level").GetComponent<LevelInfo>();
	}
}