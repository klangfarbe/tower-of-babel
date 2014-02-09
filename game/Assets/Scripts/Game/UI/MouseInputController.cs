using UnityEngine;
using System.Collections;

public class MouseInputController : BaseUIController {
	private float mouseSensitivity = 0.2f;
	private bool panningMode = false;

	private float scLeft;
	private float scMiddle;
	private float scRight;

	void Awake() {
		scLeft = 0;
		scMiddle = Screen.width / 3;
		scRight = (Screen.width / 3) * 2;
	}

	void Update () {
		// Zoom in/out
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			cameraController.zoom(-0.5f);
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			cameraController.zoom(0.5f);
		}

		// if clicked in middle of the screen start panning,
		// otherwise rotate overview
		if(Input.GetMouseButtonDown(0)) {
			panningMode = false;
			if(Input.mousePosition.x > scRight) {
				cameraController.rotateOverview(90);
			} else if(Input.mousePosition.x < scMiddle) {
				cameraController.rotateOverview(-90);
			} else {
				panningMode = true;
			}
		} else if(Input.GetMouseButton(0) && panningMode) {
			float mouseX = -Input.GetAxis("Mouse X") * mouseSensitivity;
			float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
			cameraController.translateOverview(new Vector2(mouseX, mouseY));
		}
	}
}