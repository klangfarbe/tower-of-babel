using UnityEngine;
using System.Collections;

public class MoveActor : MonoBehaviour {
	enum CurrentWalkingDirection
	{
		NORTH, EAST, SOUTH, WEST, NONE
	}

	// -----------------------------------------------------------------------------------------------------------------

	public float speed = 2.5f;
	private CurrentWalkingDirection direction = CurrentWalkingDirection.NONE;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float startTime;

	// -----------------------------------------------------------------------------------------------------------------

	void Update () {
		if(direction == CurrentWalkingDirection.NONE) {
			startTime = Time.time;
			startPosition = transform.position;
			endPosition = startPosition;
		}

		if(Input.GetKeyDown(KeyCode.W) && (direction == CurrentWalkingDirection.NONE || direction == CurrentWalkingDirection.NORTH)) {
			endPosition.z += 1;
			//transform.Rotate(new Vector3(0,
			//endPosition =
		}

		float journeyLength = Vector3.Distance(startPosition, endPosition);

		if(journeyLength != 0) {
			float distCovered = (Time.time - startTime) * speed;
        	float fracJourney = distCovered / journeyLength;
        	transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
		} else {
			direction = CurrentWalkingDirection.NONE;
		}
	}
}
