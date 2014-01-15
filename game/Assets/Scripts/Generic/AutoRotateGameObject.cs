using UnityEngine;
using System.Collections;

public class AutoRotateGameObject : MonoBehaviour {
	public float speed = 0.8f;
	public GameObject target;

	// ------------------------------------------------------------------------

	void Update () {
		float rotation = target.transform.eulerAngles.y + (speed * Time.deltaTime);
		target.transform.rotation = Quaternion.Euler (0, rotation, 0);
	}
}
