using UnityEngine;
using System.Collections;

public class MouseInputController : BaseUIController {
	private float mouseRotationSensitivity = 128;
	private float mouseSensitivity = 0.2f;
	private float mouseX = 0f;
	private float mouseY = 0f;
	private Vector2 lastMousePosition;

	void Update () {
		// Zoom in/out
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			cameraController.zoom(-0.5f);
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			cameraController.zoom(0.5f);
		}

		// Pan with left MB
		if(Input.GetMouseButton(0)) {
			mouseX = -Input.GetAxis("Mouse X") * mouseSensitivity;
			mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
			cameraController.translateOverview(new Vector2(mouseX, mouseY));
		}

		// rotate with right MB
		if(Input.GetMouseButtonDown(1)) {
			lastMousePosition = Input.mousePosition;
		}
		else if(Input.GetMouseButton(1)) {
			mouseX = Input.GetAxis("Mouse X");
			float delta = Vector2.Distance(lastMousePosition, Input.mousePosition);
			if(mouseX < 0f && delta > mouseRotationSensitivity) {
				cameraController.rotateOverview(90);
				lastMousePosition = Input.mousePosition;
			}
			if(mouseX > 0f && delta > mouseRotationSensitivity) {
				cameraController.rotateOverview(-90);
				lastMousePosition = Input.mousePosition;
			}
		}
	}
}