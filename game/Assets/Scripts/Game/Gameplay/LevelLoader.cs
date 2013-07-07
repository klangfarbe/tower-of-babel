using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

// 3rd party
using LitJson;

public class LevelLoader : MonoBehaviour {

	// -----------------------------------------------------------------------------------------------------------------
	// Attributes
	// -----------------------------------------------------------------------------------------------------------------

	public int level = 0;
	public float floorOffset = -0.03f;

	public Material patterncolor1;
	public Material patterncolor2;

	public GUIText levelName;
	public GUIText levelNr;

	private JsonData levelData;
	private int maxColumns = 0;
	private int maxRows = 0;
	private int maxFloors = 0;

	// the current fields to build
	private int floor;
	private int row;
	private int column;

	private List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

	// -----------------------------------------------------------------------------------------------------------------
	// Methods
	// -----------------------------------------------------------------------------------------------------------------

	void Start() {
		build();
	}

	// -----------------------------------------------------------------------------------------------------------------

	void Update() {
		if(Input.GetKeyUp(KeyCode.LeftArrow)) {
			if(level > 0) {
				level--;
				build();
			}
		}
		if(Input.GetKeyUp(KeyCode.RightArrow)) {
			if(level < 116) {
				level++;
				build();
			}
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void build() {
		Debug.Log("Building level...");
		clearScene();
		loadResource();
		setPatternColor();

		// create Objects
		for (floor = 0; floor < 4; floor++) { // level
			for (row = 0; row < 8; row++) { //row
				for (column = 0; column < 8; column++) { // column
					buildPosition ();
				}
			}
		}
		calculateLevelCenter();
		setLevelName();
	}

	// -----------------------------------------------------------------------------------------------------------------

	void setPatternColor() {
		patterncolor1.color = hexToColor(levelData["fx"]["patterncolor1"].ToString().Substring(2));
		patterncolor2.color = hexToColor(levelData["fx"]["patterncolor2"].ToString().Substring(2));
	}

	// -----------------------------------------------------------------------------------------------------------------

	void setLevelName() {
		levelName.text = levelData["title"].ToString();
		levelNr.text = "Level: " + level.ToString();
	}

	// -----------------------------------------------------------------------------------------------------------------

	void clearScene() {
		foreach(UnityEngine.Object o in objects) {
			Destroy(o);
		}
		objects.Clear();
		maxFloors = 0;
		maxColumns = 0;
		maxRows = 0;
		levelData = null;
	}

	// -----------------------------------------------------------------------------------------------------------------

	void loadResource() {
		Debug.Log("Loading level data...");
		TextAsset json = Resources.Load("Level/" + string.Format("{0:d3}", level)) as TextAsset;
		levelData = JsonMapper.ToObject(json.text);
	}

	// -----------------------------------------------------------------------------------------------------------------

	Color hexToColor(string hex) {
		Debug.Log(hex);
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	// -----------------------------------------------------------------------------------------------------------------

	void updateMaximumLevelDimensions() {
		maxFloors = floor > maxFloors ? floor : maxFloors;
		maxRows = row > maxRows ? row : maxRows;		maxColumns = column > maxColumns ? column : maxColumns;
	}

	// -----------------------------------------------------------------------------------------------------------------

	void calculateLevelCenter() {
		float x = maxColumns / 2f;
		float y = maxFloors / 2f;
		float z = maxRows / 2f;

		// Transform the camera and lights
		GameObject.Find("MainCameraParent").transform.position = new Vector3(x, y, z);
		GameObject.Find("Lights").transform.position = new Vector3(x, maxFloors + 1.3f, z);
	}

	// -----------------------------------------------------------------------------------------------------------------

	void buildPosition () {
		try {
			JsonData field = levelData ["elements"] [floor.ToString()] [row.ToString()] [column];
			Debug.Log("Building " + floor + "/" + row + "/" + column + ": " + field["f"].ToString() + ", " + field["o"].ToString());
			updateMaximumLevelDimensions();
			buildFloor();
			buildObject();
		} catch (Exception e) {
			//Debug.Log("Field not existent " + floor + "/" + row + "/" + column);
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	void buildObject() {
		GameObject instance = createInstance(levelData ["elements"] [floor.ToString()] [row.ToString()] [column] ["o"].ToString());
		instance.transform.parent = GameObject.Find("Actors").transform;
	}

	// -----------------------------------------------------------------------------------------------------------------

	void buildFloor() {
		var type = getFloorTypeAt(floor, row, column);
		string pattern = "";

		// select correct pattern
		if(type == "FLR" || type == "BOX") {
			if (row % 2 == 0) {
				pattern = (column % 2 == 0) ? "1" : "2";
			} else {
				pattern = (column % 2 != 0) ? "1" : "2";
			}
		}
		GameObject instance = createInstance(type + pattern, floorOffset);

		// Delete the FLR in case a BOX is below the FLR
		if(type == "FLR" && floor > 0 && getFloorTypeAt(floor - 1, row, column) == "BOX") {
			Destroy(instance);
		}

		// If the field above the box
		//Debug.Log(getFloorTypeAt(floor + 1, row, column));
		if(type == "BOX" && floor < 3 && getFloorTypeAt(floor + 1, row, column) == "FLR") {
			instance.transform.localScale += new Vector3(0, -floorOffset, 0);
		}

		instance.transform.parent = GameObject.Find("World").transform;
	}

	// -----------------------------------------------------------------------------------------------------------------

	GameObject createInstance(string type, float floorOffset = 0.0f) {
		GameObject prefab = (GameObject) Resources.Load(type);
		GameObject instance = (GameObject)Instantiate(prefab, new Vector3 (column, floor + floorOffset, row), prefab.transform.rotation);
		instance.name = type;
		objects.Add(instance);
		return instance;
	}

	// -----------------------------------------------------------------------------------------------------------------

	string getFloorTypeAt(int floor, int row, int column) {
		try {
			return levelData ["elements"] [floor.ToString()] [row.ToString()] [column] ["f"].ToString();
		} catch(Exception e) {
			Debug.LogException(e);
		}
		return null;
	}
}