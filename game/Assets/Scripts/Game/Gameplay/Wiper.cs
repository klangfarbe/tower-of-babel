using UnityEngine;
using System.Collections;

public class Wiper : Actor {
	private bool activated;
	private bool animPlayed;
	private Vector3 direction;

	private ScaleAnimation anim;
	private LevelLoader level;

	// ------------------------------------------------------------------------

	void Start() {
		level = GameObject.Find("Level").GetComponent<LevelLoader>();
		anim = GetComponent<ScaleAnimation>();
	}

	// ------------------------------------------------------------------------

	void Update() {
		if(!Enable || !activated || anim.isPlaying())
			return;

		// play animation and afterwards delete floors
		if(!animPlayed) {
			animPlayed = true;
			anim.run();
		} else {
			animPlayed = false;
			if(direction == Vector3.zero) direction = Vector3.left;
			else if(direction == Vector3.left) direction = Vector3.back;
			else if(direction == Vector3.back) direction = Vector3.right;
			else if(direction == Vector3.right) {
				direction = Vector3.forward;
				activated = false;
			}
			deleteFloors();
		}
	}

	// ------------------------------------------------------------------------

	private void deleteFloors() {
		Vector3 offset = Vector3.up * 0.25f;

		Debug.DrawRay(transform.position + offset, direction, Color.green, 0.5f);

		// calculate maximum length of the game field
		float max = direction == Vector3.left || direction == Vector3.right
			? level.MaxColumns : level.MaxRows;

		// check if a box of lift is in the way, if so adjust the maximum distance
		RaycastHit hit;
		if(Physics.Raycast(transform.position + offset, direction, out hit, max, 1 << 8)) {
			max = Vector3.Distance(transform.position, hit.point);
			Debug.Log("Wiper maximum distance: " + hit.collider.gameObject.name + " / " + hit.collider.tag + " / " +  hit.point + " / " + max);
		}

		// check every field now
		for(float i = 1; i <= max; i++) {
			Vector3 v = transform.position + i * direction + offset;
			Debug.DrawRay(v, Vector3.down, Color.green, 0.5f);

			if(Physics.Raycast(v, Vector3.down, out hit, 0.5f, 1 << 8)) {
				if(hit.collider.gameObject.name.StartsWith("FLR")) {
					Floor floor = hit.collider.gameObject.GetComponent<Floor>();
					if(floor.isFree(gameObject) && floor.assign(gameObject))
						floor.destroy();
				}
			}
		}
	}

	// ------------------------------------------------------------------------

	public override bool grabbed(GameObject by) {
		if(activated)
			return false;
		Debug.Log(gameObject.name + ": " + transform.position);
		direction = Vector3.zero;
		activated = true;
		animPlayed = false;
		return true;
	}
}