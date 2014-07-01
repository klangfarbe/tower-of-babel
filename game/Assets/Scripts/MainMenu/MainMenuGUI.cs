using UnityEngine;
using System.Collections;

public class MainMenuGUI : MonoBehaviour {
	public GUITexture background;
	public GUISkin skin;

	private AspectRatio ar;
	private Menu currentMenu = Menu.main;
	private SceneFader sceneFader;

	enum Menu { hide, main, preferences, levelselect }

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

		switch(currentMenu) {
			case Menu.main:	drawMainMenu(); break;
			case Menu.levelselect:	drawLevelSelect(); break;
		}
	}

	// ------------------------------------------------------------------------

	void drawMainMenu() {
		GUILayout.BeginArea(new Rect(ar.sWidth / 2 - 300, ar.sHeight / 2 - 400, 600, 800));
		GUILayout.BeginVertical();
		if(GUILayout.Button("Play")) {
			currentMenu = Menu.levelselect;
			//sceneFader.loadScene("game");
		}
		if(GUILayout.Button("Tutorial")) {
			Debug.Log("Tutorial");
		}
		if(GUILayout.Button("Check for updates")) {
			StartCoroutine(gameObject.GetComponent<UpdateCheck>().checkForUpdate());
		}
		if(GUILayout.Button("Exit")) {
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	// ------------------------------------------------------------------------

	void drawLevelSelect() {


	bool toggleTxt;
	int toolbarInt = 0;
	string[] toolbarStrings = {
		"Toolbar1", "Toolbar2", "Toolbar3"
	};
	int selGridInt = 0;
	string[] selStrings = {
		"Grid 1", "Grid 2", "Grid 3", "Grid 4",
		"Grid 5", "Grid 6", "Grid 7", "Grid 8", "Grid 9"
	};
	float hSliderValue = 0.0f;
	float hSbarValue;
	//	GUILayout.Box("This is the title of a box");
	//	GUILayout.Label("I'm a Label!");
	//	toggleTxt = GUILayout.Toggle(toggleTxt, "I am a Toggle button");
	//	toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarStrings);
		selGridInt = GUILayout.SelectionGrid (selGridInt, selStrings, 3);
	//	hSliderValue = GUILayout.HorizontalSlider (hSliderValue, 0.0f, 1.0f);
	//	hSbarValue = GUILayout.HorizontalScrollbar (hSbarValue, 1.0f, 0.0f, 10.0f);
	}
}
