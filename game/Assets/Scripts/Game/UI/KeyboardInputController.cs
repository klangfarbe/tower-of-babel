using UnityEngine;
using System.Collections;

public class KeyboardInputController : BaseUIController {
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			cameraController.activateGrabber();
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			cameraController.activatePusher();
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			cameraController.activateZapper();
		}
		if(Input.GetKeyDown(KeyCode.M)) {
			cameraController.activateOverview();
		}
		if(Input.GetKeyDown(KeyCode.R)) {
			StartCoroutine(gameController.levelFailed());
		}

		// Moving around
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			gameController.actorForward();
		} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			gameController.actorLeft();
		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
			gameController.actorBack();
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			gameController.actorRight();
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			gameController.actorFire();
		} else if(Input.GetKeyDown(KeyCode.Return)) {
			gameController.actorLift();
		}

		// Level loader
	}
}