using UnityEngine;
using System.Collections;

public class GUIButtons : MonoBehaviour {
	public SwitchCamera cameraSelector;

	public Texture texZapper;
	public Texture texGrabber;
	public Texture texPusher;
	public Texture texForward;
	public Texture texBack;
	public Texture texLeft;
	public Texture texRight;
	public Texture texUpDown;
	public Texture texFire;
	public GUISkin skin;

	private float sWidth = 1024f;
	private float sHeight = 768f;

	void OnGUI() {
		calculateAspectRatio();

		float xFactor = Screen.width / sWidth;
		float yFactor = Screen.height / sHeight;
		GUIUtility.ScaleAroundPivot(new Vector2(xFactor, yFactor), Vector2.zero);

		GUI.skin = skin;
		GUILayout.BeginArea(new Rect(sWidth / 2 - 320, sHeight - 80, 640, 70));
		GUILayout.BeginHorizontal();
        GUILayout.Button(texGrabber);
        GUILayout.Button(texPusher);
        GUILayout.Button(texZapper);
        GUILayout.Button(texFire);
        GUILayout.Button(texLeft);
        GUILayout.Button(texForward);
        GUILayout.Button(texBack);
        GUILayout.Button(texRight);
        GUILayout.Button(texUpDown);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
	}

	// ------------------------------------------------------------------------

	void calculateAspectRatio () {
		// calculate aspect ratio
		var aspect = (float)Screen.width / (float)Screen.height;
		if(aspect >= 1.7f && aspect < 1.78f) {
			sWidth = 1280;
			sHeight = 720;
		} else if(aspect >= 1.3f && aspect < 1.34f) {
			sWidth = 1024;
			sHeight = 768;
		}
		Debug.Log("Screen aspect ratio is " + aspect.ToString("F4") + " with size " + Screen.width + "x" + Screen.height);
	}
}
