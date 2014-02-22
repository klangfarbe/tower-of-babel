﻿using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {
	public GUITexture background;
	public GUISkin skin;

	private AspectRatio ar;
	private Menu currentMenu = Menu.main;
	private SceneFader sceneFader;

	enum Menu { main, preferences, levelselect }

	// ------------------------------------------------------------------------

	void Start() {
		sceneFader = SceneFader.create();
		sceneFader.startScene();
		ar = new AspectRatio();
	}

	// ------------------------------------------------------------------------

	void centerBackground() {
		float offX = -((background.texture.width - ar.sWidth) / 2);
		float offY = -((background.texture.height - ar.sHeight) / 2);
		background.pixelInset = new Rect(
			offX * ar.xFactor,
			offY * ar.yFactor,
			background.texture.width * ar.xFactor,
			background.texture.height * ar.yFactor);
	}

	// ------------------------------------------------------------------------

	void OnGUI() {
		GUI.skin = skin;
		ar.initGuiScale();
		centerBackground();

		if(currentMenu == Menu.main) {
			drawMainMenu();
		}
	}

	// ------------------------------------------------------------------------

	void drawMainMenu() {
		GUILayout.BeginArea(new Rect(ar.sWidth / 2 - 400, ar.sHeight / 2 - 400, 800, 800));
		GUILayout.BeginVertical();
		if(GUILayout.Button("Play")) {
			sceneFader.loadScene("game");
		}
		if(GUILayout.Button("Tutorial")) {
			Debug.Log("Tutorial");
		}
		if(GUILayout.Button("Game elements")) {

		}
		if(GUILayout.Button("Tower Designer")) {

		}
		if(GUILayout.Button("Download new level")) {

		}
		if(GUILayout.Button("Exit")) {

		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}


//	bool toggleTxt;
//	int toolbarInt = 0;
//	string[] toolbarStrings = {
//		"Toolbar1", "Toolbar2", "Toolbar3"
//	};
//	int selGridInt = 0;
//	string[] selStrings = {
//		"Grid 1", "Grid 2", "Grid 3", "Grid 4"
//	};
//	float hSliderValue = 0.0f;
//	float hSbarValue;
//		GUILayout.Box("This is the title of a box");
//		GUILayout.Label("I'm a Label!");
//		toggleTxt = GUILayout.Toggle(toggleTxt, "I am a Toggle button");
//		toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarStrings);
//		selGridInt = GUILayout.SelectionGrid (selGridInt, selStrings, 2);
//		hSliderValue = GUILayout.HorizontalSlider (hSliderValue, 0.0f, 1.0f);
//		hSbarValue = GUILayout.HorizontalScrollbar (hSbarValue, 1.0f, 0.0f, 10.0f);
}