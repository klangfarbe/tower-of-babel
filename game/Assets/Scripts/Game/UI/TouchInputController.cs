using UnityEngine;
using System.Collections;

public class TouchInputController : BaseUIController {
#if UNITY_IPHONE || UNITY_ANDROID
	private float sensitivityPanning = 0.01f;
	private float sensitivityAngle = 0.2f;
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
//		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
//			cameraController.zoom(-0.5f);
//		}
//		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
//			cameraController.zoom(0.5f);
//		}
//
		// if clicked in middle of the screen start panning,
		// otherwise rotate overview
		if(Input.touchCount == 1) {
			Touch t = Input.touches[0];
			if(t.phase == TouchPhase.Began) {
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
			} else if(t.phase == TouchPhase.Moved && panningMode) {
				float x = -t.deltaPosition.x * sensitivityPanning;
				float y = -t.deltaPosition.y * sensitivityPanning;
				cameraController.translateOverview(new Vector2(x,y));
			} else if(t.phase == TouchPhase.Moved && angleChangeMode) {
				cameraController.changeCameraAngle(-t.deltaPosition.y * sensitivityAngle);
			}
		}
	}
#endif
}