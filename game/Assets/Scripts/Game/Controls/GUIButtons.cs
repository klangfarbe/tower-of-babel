using UnityEngine;
using System.Collections;

public class GUIButtons : MonoBehaviour {
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

	void OnGUI() {
		GUI.skin = skin;
		GUILayout.BeginArea(new Rect(Screen.width - 720, Screen.height - 560, 200, 160));
		GUILayout.BeginHorizontal();
        GUILayout.Button(texUpDown);
        GUILayout.Button(texForward);
        GUILayout.Button(texFire);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Button(texLeft);
        GUILayout.Button(texBack);
        GUILayout.Button(texRight);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
