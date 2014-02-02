using UnityEngine;
using System.Collections;

public class Reflector : Actor {
	public override bool zapped(GameObject by) {
		if(by) {
			return by.GetComponent<Actor>().zapped(gameObject);
		}
		return false;
	}
}