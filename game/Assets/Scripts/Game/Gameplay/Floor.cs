using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {
	public GameObject objectOnFloor;

	// ------------------------------------------------------------------------

	public void release(GameObject g) {
		if(!objectOnFloor || objectOnFloor != g)
			return;
		Debug.Log(gameObject.name + ": released");
		objectOnFloor = null;
	}

	// ------------------------------------------------------------------------

	public void assign(GameObject g) {
		if(objectOnFloor == g)
			return;
		Debug.Log(gameObject.name + ": assigned by " + g.name);
		objectOnFloor = g;
	}

	// ------------------------------------------------------------------------

	public bool isFree(GameObject g) {
		return objectOnFloor == null || objectOnFloor == g;
	}
}