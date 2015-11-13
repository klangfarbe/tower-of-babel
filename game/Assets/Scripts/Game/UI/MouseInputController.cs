using UnityEngine;
using System.Collections;

public class MouseInputController : BaseUIController {
	protected float sensitivityPanning = 0.2f;
	protected float sensitivityAngle = 0.4f;
	protected bool panningMode = false;
	protected bool angleChangeMode = false;

	// helper lines which divide the screen
	protected float[] vLines;// = new float[5];
	protected float[] hLines = new float[5];

	private int sWidth = 0;
	private int sHeight = 0;

	// ------------------------------------------------------------------------

	protected enum Quadrant {
		deadzone,
		rotateLeft,
		rotateRight,
		pan,
		angle
	}

	// ------------------------------------------------------------------------

	protected void Awake() {
		UpdateVLines();
	}

	// ------------------------------------------------------------------------

	protected void UpdateVLines() {
		if(sWidth == Screen.width && sHeight == Screen.height)
			return;

		float deadzone = 10;
		vLines = new float[] {
			deadzone,
			Screen.width / 4,
			(Screen.width / 4) * 3,
			Screen.width - deadzone,
			Screen.width
		};
		hLines[0] = Screen.height / 3;

		sWidth = Screen.width;
		sHeight = Screen.height;

		if(Debug.isDebugBuild) {
			Debug.Log("hLines: " + hLines[0]);
			Debug.Log("vLines: " + vLines[0] + ", " + vLines[1] + ", " + vLines[2] + ", " + vLines[3] + ", " + vLines[4]);
		}
	}

	// ------------------------------------------------------------------------

	protected Quadrant interpretPosition(Vector2 p) {
		panningMode = false;
		angleChangeMode = false;

		if(p.x > vLines[0] && p.x < vLines[1]) {
			cameraController.rotateOverview(-90);
			return Quadrant.rotateLeft;
		}
		if(p.x > vLines[1] && p.x < vLines[2] && p.y > hLines[0]) {
			panningMode = true;
			return Quadrant.pan;
		}
		if(p.x > vLines[1] && p.x < vLines[2] && p.y < hLines[0]) {
			angleChangeMode = true;
			return Quadrant.angle;
		}
		if(p.x > vLines[2] && p.x < vLines[3]) {
			cameraController.rotateOverview(90);
			return Quadrant.rotateRight;
		}
		return Quadrant.deadzone;
	}

	// ------------------------------------------------------------------------

	void Update () {
#if UNITY_STANDALONE || UNITY_WEBPLAYER  || UNITY_WEBGL
		UpdateVLines();

		// Zoom in/out
		if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			cameraController.zoom(-0.5f);
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0) {
			cameraController.zoom(0.5f);
		}

		if(Input.GetMouseButtonDown(0)) {
			interpretPosition(Input.mousePosition);
		} else if(Input.GetMouseButton(0) && panningMode) {
			float mouseX = -Input.GetAxis("Mouse X") * sensitivityPanning;
			float mouseY = -Input.GetAxis("Mouse Y") * sensitivityPanning;
			cameraController.translateOverview(new Vector2(mouseX, mouseY));
		} else if(Input.GetMouseButton(0) && angleChangeMode) {
			cameraController.changeCameraAngle(-Input.GetAxis("Mouse Y") * sensitivityAngle);
		}
#endif
	}
}