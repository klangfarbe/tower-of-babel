using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	private bool enable = false;
	protected GameController gameController;

	// ------------------------------------------------------------------------

	void Awake () {
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
	}

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

	public virtual bool fire() {
		if(!Enable)
			Enable = true;
		return true;
	}

	// ------------------------------------------------------------------------

	public virtual bool grabbed(GameObject by) {
		return false;
	}

	// ------------------------------------------------------------------------

	public virtual bool zapped(GameObject by) {
		DestroyActor m = GetComponent<DestroyActor>();
		if(m && !isOnMovingLift()) {
			m.destroy();
			return true;
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public virtual bool pushed(GameObject by) {
		if(!Enable)
			Enable = true;
		MoveActor m = GetComponent<MoveActor>();
		if(m) {
			return m.push(by);
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public virtual bool move(Vector3 direction) {
		if(!Enable)
			Enable = true;
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			return m.move(direction);
		return false;
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

	public virtual void lift() {
		if(!Enable)
			Enable = true;
		MoveActor m = GetComponent<MoveActor>();
		if(m)
			m.lift();
	}

	// ------------------------------------------------------------------------

	public bool raycast(out RaycastHit hit) {
		if(Debug.isDebugBuild) {
			Debug.DrawRay(transform.position + Vector3.up * 0.25f, transform.forward * 10, Color.yellow, 0.2f);
		}

		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, transform.forward, out hit)) {
			return true;
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public void playAudio(int index = 0) {
		AudioSource[] audiofiles = gameObject.GetComponents<AudioSource>();
		if(audiofiles != null && audiofiles.Length >= index) {
			Debug.Log("playing audio file " + index);
			audiofiles[index].Play();
		}
	}

	// ------------------------------------------------------------------------

	public bool isOnLift() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f))
			if(hit.collider.tag == "Lift")
				return true;
		return false;
	}

	// ------------------------------------------------------------------------

	protected bool isOnMovingLift() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f))
			if(hit.collider.tag == "Lift")
				return hit.collider.gameObject.GetComponent<Lift>().isPlaying();
		return false;
	}
}