using UnityEngine;
using System.Collections;

public class BaseUIController : MonoBehaviour {
	protected CameraController cameraController;
	protected GameController gameController;
	protected Conditions conditions;
	protected LevelLoader level;

	protected AspectRatio ar;

	// ------------------------------------------------------------------------

	void Awake() {
		ar = new AspectRatio();

		cameraController = GameObject.Find("Controller").GetComponent<CameraController>();
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
		conditions = GameObject.Find("Level").GetComponent<Conditions>();
		level = GameObject.Find("Level").GetComponent<LevelLoader>();
		if(Debug.isDebugBuild)
			Debug.Log("Called BaseUIController Awake: cam " + cameraController + ", game "
				+ gameController + ", conditions " + conditions + ", level " + level);
	}
}