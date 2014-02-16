using UnityEngine;
using System.Collections;

public class TouchInputController : MouseInputController {
	new void Awake() {
		base.Awake();
		sensitivityAngle = 0.3f;
		sensitivityPanning = 0.025f;
	}

#if UNITY_IPHONE || UNITY_ANDROID
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
				interpretPosition(Input.mousePosition);
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