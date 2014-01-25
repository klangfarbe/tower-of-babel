using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	private bool enable = false;

	// ------------------------------------------------------------------------

	public bool Enable {
		get {
			return this.enable;
		}
		set {
			this.enable = value;
			if(enable) {
				GameObject.Find("Level").GetComponent<Conditions>().startLevel();
			}
		}
	}

	// ------------------------------------------------------------------------

	public virtual void fire() {
		if(!Enable)
			Enable = true;
	}

	// ------------------------------------------------------------------------

	public virtual void grabbed(GameObject by) {}

	// ------------------------------------------------------------------------

	public virtual void zapped(GameObject by) {
		DestroyActor m = GetComponent<DestroyActor>();
		if(m)
			m.destroy();
	}

	// ------------------------------------------------------------------------

	public virtual void pushed(GameObject by) {
		if(!Enable)
			Enable = true;
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			m.push(by);
	}

	// ------------------------------------------------------------------------

	public virtual void move(Vector3 direction) {
		if(!Enable)
			Enable = true;
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			m.move(direction);
	}

	// ------------------------------------------------------------------------

	public virtual void turnRight() {
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			m.turnRight();
	}

	// ------------------------------------------------------------------------

	public virtual void turnLeft() {
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			m.turnLeft();
	}

	// ------------------------------------------------------------------------

	public virtual void up() {
		if(!Enable)
			Enable = true;
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			m.up();
	}

	// ------------------------------------------------------------------------

	public virtual void down() {
		if(!Enable)
			Enable = true;
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			m.down();
	}

	// ------------------------------------------------------------------------

	public bool raycast(out RaycastHit hit) {
		Debug.DrawRay(transform.position + Vector3.up * 0.25f, transform.forward * 10, Color.yellow, 0.2f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, transform.forward, out hit)) {
			//Debug.Log("Fire: " + hit.collider.gameObject.name + " " + hit.collider.tag);
			return true;
		}
		return false;
	}
}