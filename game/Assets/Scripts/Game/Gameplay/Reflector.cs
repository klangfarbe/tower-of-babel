using UnityEngine;
using System.Collections;

public class Reflector : Actor {
	public override void zapped(GameObject by) {
		if(by) {
			by.GetComponent<Actor>().zapped(gameObject);
		}
	}
}