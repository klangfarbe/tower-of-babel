using UnityEngine;
using System.Collections;

public class MouseInputController : BaseUIController {
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			cameraController.zoom(-0.5f);
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			cameraController.zoom(0.5f);
		}
	}
}