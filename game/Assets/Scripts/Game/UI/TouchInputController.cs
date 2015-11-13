using UnityEngine;
using System.Collections;

public class TouchInputController : MouseInputController {
#if UNITY_IOS || UNITY_ANDROID
	private float minPinchSpeed = 0.75F;
	private float varianceInDistances = 2.0F;
	private float touchDelta = 0.0F;
	private float speedTouch0 = 0.0F;
	private float speedTouch1 = 0.0F;
	private Vector2 prevDist = new Vector2(0,0);
	private Vector2 curDist = new Vector2(0,0);

	new void Awake() {
		base.Awake();
		sensitivityAngle = 0.3f;
		sensitivityPanning = 0.025f;
	}

	void Update () {
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

		if(Input.touchCount == 2
			&& Input.GetTouch(0).phase == TouchPhase.Moved
			&& Input.GetTouch(1).phase == TouchPhase.Moved) {
			curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;

			// calculate previous position with the deltaPosition of each touch
			// afterwards, calculate the previous distance between the touches
			prevDist = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition)
						- (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

			touchDelta = curDist.magnitude - prevDist.magnitude;
			speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
			speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

			if((touchDelta + varianceInDistances < 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed)) {
				cameraController.zoom(0.2f);
			}
			if((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed)) {
				cameraController.zoom(-0.2f);
			}
		}
	}
#endif
}