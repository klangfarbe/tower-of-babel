using UnityEngine;
using System.Collections;

public class Converter : Actor {
	public Vector3 direction;

	// ------------------------------------------------------------------------

	public override void pushed(GameObject by) {
		Actor target = raycast(by.transform.forward);
		if(target) {
			target.zapped(by);
		}
	}

	// ------------------------------------------------------------------------

	public override void zapped(GameObject by) {
		Actor target = raycast(by.transform.forward);
		if(target) {
			target.pushed(by);
		}
	}

	// ------------------------------------------------------------------------

	public Actor raycast(Vector3 direction) {
		if(direction == this.direction || direction == -this.direction) {
			RaycastHit hit;
			Debug.DrawRay(transform.position + Vector3.up * 0.25f, direction * 10, Color.red, 0.2f);
			if(Physics.Raycast(transform.position + Vector3.up * 0.25f, direction, out hit)) {
				if(hit.collider.tag == "Actor" || hit.collider.tag == "Player") {
					return hit.collider.gameObject.GetComponent<Actor>();
				}
			}
		}
		return null;
	}
}