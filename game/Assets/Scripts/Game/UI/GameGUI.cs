using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGUI : BaseUIController {
	public Texture2D texMap;
	public Texture2D texGrabber;
	public Texture2D texPusher;
	public Texture2D texZapper;
	public Texture2D texForward;
	public Texture2D texBack;
	public Texture2D texLeft;
	public Texture2D texRight;
	public Texture2D texUpDown;
	public Texture2D texFire;
	public Texture2D texPause;
	public Texture2D texWndBackground;
	public Font font;

	private bool drawPauseMenu = false;

	private GUIStyle btnStyle = new GUIStyle();
	private GUIStyle wndStyle = new GUIStyle();
	private GUIStyle wndBtnStyle = new GUIStyle();
	private GUIStyle cndTextStyle = new GUIStyle();

	// ------------------------------------------------------------------------

	void Awake() {
		setupGUIStyles();
	}

	// ------------------------------------------------------------------------

	void OnGUI() {
		if(ar == null) {
			ar = new AspectRatio();
		}
		ar.initGuiScale();
		mobileGUI();
	}

	// ------------------------------------------------------------------------

	private void setupGUIStyles() {
		wndStyle.normal.background = texWndBackground;
		wndStyle.border = new RectOffset(3,3,3,3);
		wndStyle.padding = new RectOffset(25,25,25,25);

		btnStyle.fixedHeight = 96;
		btnStyle.fixedWidth = 96;
		btnStyle.margin.bottom = 10;
		btnStyle.margin.right = 10;
		btnStyle.alignment = TextAnchor.MiddleRight;

		wndBtnStyle.fontSize = 64;
		wndBtnStyle.font = font;
		wndBtnStyle.normal.textColor = Color.white;
		wndBtnStyle.alignment = TextAnchor.UpperCenter;
		wndBtnStyle.margin = new RectOffset(25,25,25,25);

		cndTextStyle.fontSize = 36;
		cndTextStyle.font = font;
		cndTextStyle.normal.textColor = Color.white;
		cndTextStyle.alignment = TextAnchor.UpperLeft;
	}

	// ------------------------------------------------------------------------

	void mobileGUI() {
		float btnFullWidth = btnStyle.fixedWidth + btnStyle.margin.right;
		float btnFullHeight = btnStyle.fixedHeight + btnStyle.margin.bottom;

		// Overview buttons
		GUILayout.BeginArea(new Rect(10, ar.sHeight - btnFullHeight, btnFullWidth * 4, btnFullHeight));
		GUILayout.BeginHorizontal();
		if(level.Cameras && GUILayout.Button(texMap, btnStyle)) {
			cameraController.activateOverview();
		}
		if(GameObject.Find("GRB") && GUILayout.Button(texGrabber, btnStyle)) {
			cameraController.activateGrabber();
		}
		if(GameObject.Find("PSH") && GUILayout.Button(texPusher, btnStyle)) {
			cameraController.activatePusher();
		}
		if(GameObject.Find("ZAP") && GUILayout.Button(texZapper, btnStyle)) {
			cameraController.activateZapper();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();

		// Overview buttons on the right side of the screen
		if(!cameraController.mapActive) {
			GUILayout.BeginArea(new Rect(10, ar.sHeight - btnFullHeight * 2, btnFullWidth * 4, btnFullHeight));
			GUILayout.BeginHorizontal();
			if(GUILayout.Button(texFire, btnStyle)) {
				gameController.actorFire();
			}
			if(GUILayout.Button(texLeft, btnStyle)) {
				gameController.actorLeft();
			}
			if(GUILayout.Button(texForward, btnStyle)) {
				gameController.actorForward();
			}
			if(GUILayout.Button(texRight, btnStyle)) {
				gameController.actorRight();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();

			// second line
			if(gameController.activeObject != null && gameController.activeObject.GetComponent<Actor>().isOnLift()) {
				GUILayout.BeginArea(new Rect(10 + btnFullWidth * 2, ar.sHeight - btnFullHeight * 3, btnFullWidth, btnFullHeight));
				if(GUILayout.Button(texUpDown, btnStyle)) {
					gameController.actorLift();
				}
				GUILayout.EndArea();
			}
		}

		// Pause button
		if(GUI.Button(new Rect(ar.sWidth - btnFullWidth, 10, btnFullWidth, btnFullHeight), texPause, btnStyle)) {
			drawPauseMenu = true;
		}

		// conditions
		GUI.Label(new Rect(10, 10, 150, 50), conditions.getConditionsText(), cndTextStyle);

		// remaining Time
		GUI.Label(new Rect(ar.sWidth / 2 - 100, 10, 200, 150), conditions.getRemainingTime(), wndBtnStyle);

		if(drawPauseMenu) {
			GUI.ModalWindow(0, new Rect(ar.sWidth / 2 - 480, ar.sHeight / 2 - 235, 960, 470), pauseMenu, "", wndStyle);
		}
	}

	// ------------------------------------------------------------------------

	void pauseMenu(int windowId) {
		gameController.levelPause();

		GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
		if(GUILayout.Button("Restart Tower", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			drawPauseMenu = false;
			gameController.levelUnpause();
			StartCoroutine(gameController.levelRestart());
		}
		if(GUILayout.Button("Main Menu", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			drawPauseMenu = false;
			gameController.levelUnpause();
			StartCoroutine(gameController.levelAbort());
		}
		if(GUILayout.Button("Resume Game", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			drawPauseMenu = false;
			gameController.levelUnpause();
		}
		if(GUILayout.Button("Exit", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			drawPauseMenu = false;
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.Label("Version " + (((TextAsset)Resources.Load("version")).text), cndTextStyle);
	}
}
