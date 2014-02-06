using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public void actorFire() {

	}

	// ------------------------------------------------------------------------

	public void actorLift() {

	}

	// ------------------------------------------------------------------------

	public void actorLeft() {

	}

	// ------------------------------------------------------------------------

	public void actorForward() {

	}

	// ------------------------------------------------------------------------

	public void actorBack() {

	}

	// ------------------------------------------------------------------------

	public void actorRight() {

	}

	// ------------------------------------------------------------------------

	public IEnumerator levelCompleted() {
		yield return new WaitForSeconds(5);
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelFailed() {
		yield return new WaitForSeconds(5);
	}

	// ------------------------------------------------------------------------

	public void levelPause() {

	}

	// ------------------------------------------------------------------------


}