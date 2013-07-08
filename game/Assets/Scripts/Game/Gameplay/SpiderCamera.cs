using UnityEngine;
using System.Collections;

public class SpiderCamera : MonoBehaviour {
	public Transform target;
	public float distance = 1;

	// -----------------------------------------------------------------------------------------------------------------

	void LateUpdate () {
		if(!target) {
			return;
		}

		var currentRotation = Quaternion.Euler (0, target.eulerAngles.y, 0);
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		transform.LookAt(target);
	}
}
