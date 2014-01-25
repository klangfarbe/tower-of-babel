using UnityEngine;
using System.Collections;

public class Prism : Actor {
	public Vector3 input;
	public Vector3 output;

	// ------------------------------------------------------------------------

	public override void zapped(GameObject by) {
		Actor target = null;

		if(by.transform.forward == input) {
			target = raycast(output);
		} else if(by.transform.forward == -output) {
			target = raycast(-input);
		}

		if(target) {
			target.zapped(by);
		}
	}

	// ------------------------------------------------------------------------

	public Actor raycast(Vector3 direction) {
		RaycastHit hit;
		Debug.DrawRay(transform.position + Vector3.up * 0.25f, direction * 10, Color.red, 0.2f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, direction, out hit)) {
			if(hit.collider.tag == "Actor" || hit.collider.tag == "Player") {
				return hit.collider.gameObject.GetComponent<Actor>();
			}
		}
		return null;
	}
}