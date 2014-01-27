using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
	public GameObject objectOnFloor;

	// ------------------------------------------------------------------------

	public void release(GameObject g) {
		if(!objectOnFloor || objectOnFloor != g)
			return;
		Debug.Log(gameObject.name + ": released by " + g.name);
		objectOnFloor = null;
	}

	// ------------------------------------------------------------------------

	public bool assign(GameObject g) {
		if(objectOnFloor == g)
			return true;
		if(objectOnFloor == null) {
			Debug.Log(gameObject.name + ": assigned by " + g.name);
			objectOnFloor = g;
			return true;
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public bool isFree(GameObject g) {
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.down * 0.1f, Vector3.up, out hit, 0.5f, 1 << 9)) {
			Debug.Log(gameObject.name);
			return hit.collider.gameObject == g;
		}
		return objectOnFloor == null || objectOnFloor == g;
	}
}