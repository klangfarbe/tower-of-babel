using UnityEngine;
using System.Collections;

public class BaseUIController : MonoBehaviour {
	protected CameraController cameraController;
	protected GameController gameController;
	protected Conditions conditions;

	// ------------------------------------------------------------------------

	void Start() {
		cameraController = GameObject.Find("Controller").GetComponent<CameraController>();
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
		conditions = GameObject.Find("Level").GetComponent<Conditions>();
	}
}