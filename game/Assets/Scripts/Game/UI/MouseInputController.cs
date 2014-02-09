using UnityEngine;
using System.Collections;

public class MouseInputController : BaseUIController {
#if UNITY_STANDALONE
	private float mouseSensitivity = 0.2f;
	private bool panningMode = false;
	private bool angleChangeMode = false;

	private float scLeft;
	private float scMiddle;
	private float scRight;
	private float scHorizontal;

	void Awake() {
		scLeft = 0;
		scMiddle = Screen.width / 3;
		scRight = (Screen.width / 3) * 2;
		scHorizontal = Screen.height / 2;
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
			angleChangeMode = false;
			if(Input.mousePosition.x > scRight) {
				cameraController.rotateOverview(90);
			} else if(Input.mousePosition.x < scMiddle) {
				cameraController.rotateOverview(-90);
			} else {
				if(Input.mousePosition.y > scHorizontal) {
					panningMode = true;
				} else {
					angleChangeMode = true;
				}
			}
		} else if(Input.GetMouseButton(0) && panningMode) {
			float mouseX = -Input.GetAxis("Mouse X") * mouseSensitivity;
			float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
			cameraController.translateOverview(new Vector2(mouseX, mouseY));
		} else if(Input.GetMouseButton(0) && angleChangeMode) {
			cameraController.changeCameraAngle(-Input.GetAxis("Mouse Y"));
		}
	}
#endif
}