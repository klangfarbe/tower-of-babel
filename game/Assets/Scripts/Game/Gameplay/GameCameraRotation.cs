using UnityEngine;
using System.Collections;

public class GameCameraRotation : MonoBehaviour {
	public Transform lookAtObject;
	public float maxAngleMin = 0;
	public float maxAngleMax = 80;
	public float rotationSpeedX = 1.0f;
	public float rotationSpeedY = 1.0f;
	public float distanceMin = 1;
	public float distanceMax = 1;
		
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
