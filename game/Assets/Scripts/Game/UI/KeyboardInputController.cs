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

		if(cameraController.mapActive) {
			// Overview
			if(!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.UpArrow)) {
				cameraController.translateOverview(Vector3.up * 0.1f);
			}
			if(!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.DownArrow)) {
				cameraController.translateOverview(-Vector3.up * 0.1f);
			}
			if(!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftArrow)) {
				cameraController.translateOverview(Vector3.left * 0.1f);
			}
			if(!Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightArrow)) {
				cameraController.translateOverview(-Vector3.left * 0.1f);
			}
			if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.LeftArrow)) {
				cameraController.rotateOverview(-90);
			}
			if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.RightArrow)) {
				cameraController.rotateOverview(90);
			}
			if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.UpArrow)) {
				cameraController.zoom(-0.1f);
			}
			if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.DownArrow)) {
				cameraController.zoom(0.1f);
			}
		} else {
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
		}

		// Level loader
	}
}