using UnityEngine;
using System.Collections;

public class SpiderCamera : MonoBehaviour {
	public Transform target;
	public float height = 0;
	public float distance = 1;
	public float rotationDamping = 1;

	// -----------------------------------------------------------------------------------------------------------------

	void LateUpdate () {
		if(!target) {
			return;
		}

		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;

		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
//		transform.position.y = height;

		// Always look at the target
		transform.LookAt(target);
	}
}
