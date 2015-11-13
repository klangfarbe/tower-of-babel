using UnityEngine;
using System.Collections;

public class Prism : Actor {
	public Vector3 input;
	public Vector3 output;

	// ------------------------------------------------------------------------

	public override bool zapped(GameObject by) {
		Actor target = null;

		var heading = transform.position - by.transform.position;
		var direction = heading / heading.magnitude;

		if(Debug.isDebugBuild) {
			Debug.Log("Prism zapped by " + by.name + " from " + by.transform.position
				+ " with direction " + direction + " (in " + input + " / out " + output + ")");
		}

		if(direction == input) {
			target = raycast(output);
		} else if(direction == -output) {
			target = raycast(-input);
		}

		if(target) {
			return target.zapped(gameObject);
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public Actor raycast(Vector3 direction) {
		RaycastHit hit;
		if(Debug.isDebugBuild) {
			Debug.DrawRay(transform.position + Vector3.up * 0.25f, direction * 10, Color.red, 0.2f);
		}

		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, direction, out hit)) {
			if(hit.collider.tag == "Actor" || hit.collider.tag == "Player") {
				return hit.collider.gameObject.GetComponent<Actor>();
			}
		}
		return null;
	}
}