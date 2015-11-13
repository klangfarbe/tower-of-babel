using UnityEngine;
using System.Collections;

public class Exchanger : Reflector {
	public override bool grabbed(GameObject by) {
		MoveActor actor = by.GetComponent<MoveActor>();
		Vector3 tmp = actor.transform.position;
		actor.set(transform.position);
		by.transform.RotateAround(by.transform.position, Vector3.up, 180);
		transform.position = tmp;
		GetComponent<DestroyActor>().destroy();
		return true;
	}
}