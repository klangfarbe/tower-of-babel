using UnityEngine;
using System.Collections;

public class Klondike : Actor {
	public override void grabbed(GameObject by) {
		gameObject.SetActive(false);
		//Destroy(this.gameObject);
		GameObject.Find("Level").GetComponent<Conditions>().pickupKlondike();
	}
}