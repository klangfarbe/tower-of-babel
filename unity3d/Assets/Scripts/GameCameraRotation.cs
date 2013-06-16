using UnityEngine;
using System.Collections;

public class GameCameraRotation : MonoBehaviour {
	public Transform targetX;
	public Transform targetY;
	public float maxAngle = 40.0f;
	public float rotationSpeedX = 1.0f;
	public float rotationSpeedY = 2.0f;	
	public float mouseDeadZone = 0.2f;
		
	void Update () {
		/*
		if(!targetY || !targetX)
			return;
		
		float rotateDegrees = 0f;
		
		if(Input.GetMouseButton(2)) {
			return;
		}
		*/
	}
}
