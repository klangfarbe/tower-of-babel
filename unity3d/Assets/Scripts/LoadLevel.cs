using UnityEngine;
using System.Collections.Generic;

using LitJson;
using System;
using System.IO;

public class LoadLevel : MonoBehaviour {
	public int level = 0;
	public float floorOffset = 0.0f;
	public float klondikeHeight = 0.15f;
	public GameObject grabber;
	public GameObject pusher;
	public GameObject zapper;
	public GameObject floorPattern1;
	public GameObject floorPattern2;
	public GameObject boxPattern1;
	public GameObject boxPattern2;
	public GameObject liftDown;
	public GameObject liftUp;
	public GameObject klondike;
	public GameObject block;
	public GameObject reflector;
	public GameObject prism;
	public GameObject converter;

	public Material materialPattern1;
	public Material materialPattern2;

	public GUIText levelName;
	public GUIText levelNr;

	private JsonData levelData;
	private int maxColumns = 0;
	private int maxRows = 0;
	private int maxFloors = 0;

	private List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

	// ------------------------------------------------------------------------

	void Start() {
		Debug.Log("Awakening scene...");
		build();
	}

	// ------------------------------------------------------------------------

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

	// ------------------------------------------------------------------------

	public void build() {
		Debug.Log("Building level...");
		clearScene();
		loadResource();
		setMaterialColors();

		// create Objects
		for (int floor = 3; floor >= 0; floor--) { // level
			for (int row = 7; row >= 0; row--) { //row
				for (int column = 7; column >= 0; column--) { // column
					buildPosition (floor, row, column);
				}
			}
		}
		calculateLevelCenter();
		setLevelName();
	}

	void setLevelName() {
		levelName.text = levelData["title"].ToString();
		levelNr.text = "Level: " + level.ToString();
	}

	// ------------------------------------------------------------------------

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

	// ------------------------------------------------------------------------
	
	void loadResource() {
		Debug.Log("Loading level data...");
		TextAsset json = Resources.Load("Level/" + string.Format("{0:d3}", level)) as TextAsset;
		levelData = JsonMapper.ToObject(json.text);
	}

	// ------------------------------------------------------------------------

	void setMaterialColors() {
		materialPattern1.color = hexToColor(levelData["fx"]["patterncolor1"].ToString().Substring(2));
		materialPattern2.color = hexToColor(levelData["fx"]["patterncolor2"].ToString().Substring(2));
		Debug.Log(materialPattern1.color.ToString());
		Debug.Log(materialPattern2.color.ToString());
	}

	// ------------------------------------------------------------------------

	Color hexToColor(string hex) {
		Debug.Log(hex);
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	// ------------------------------------------------------------------------

	void updateMaximumLevelDimensions(int floor, int row, int column) {
		maxFloors = floor > maxFloors ? floor : maxFloors;
		maxRows = row > maxRows ? row : maxRows;
		maxColumns = column > maxColumns ? column : maxColumns;
	}

	// ------------------------------------------------------------------------

	void calculateLevelCenter() {
		float x = maxColumns / 2f;
		float y = maxFloors / 2f;
		float z = maxRows / 2f;
		Debug.Log("Position: " + y + ", field: " + x + ", " + z);

		// Transform the camera and lights
		GameObject.Find("MainCameraParent").transform.position = new Vector3(x, y, z);
		GameObject.Find("Lights").transform.position = new Vector3(x, maxFloors + 2, z);
	}

	// ------------------------------------------------------------------------
	
	void buildPosition (int floor, int row, int column) {
		try {
			JsonData field = levelData ["elements"] [floor] [row] [column];
			updateMaximumLevelDimensions(floor, row, column);
			Debug.Log("Building " + floor + "/" + row + "/" + column);

			buildFloor(field["f"].ToString(), floor, maxRows - row, column);
			buildObject(field["o"].ToString(), floor, maxRows - row, column);
		} catch (Exception e) {
			//Debug.LogException(e);
		}
	}

	// ------------------------------------------------------------------------

	void buildObject(string type, int floor, int row, int column) {
		UnityEngine.Object obj = null;
		Quaternion rotation = Quaternion.identity;

		switch(type) {
		case "GRB":
			obj = grabber; break;
		case "PSH":
			obj = pusher; break;
		case "ZAP":
			obj = zapper; break;
		case "KLD":
			obj = klondike; break;
		case "BLK":
			obj = block; break;
		case "REL":
			obj = reflector; break;
		case "PSW":
			rotation = Quaternion.Euler(0, 0, 0);
			obj = prism; break;
		case "PNW":
			rotation = Quaternion.Euler(0, 90.0f, 0);
			obj = prism; break;
		case "PNE":
			rotation = Quaternion.Euler(0, 180.0f, 0);
			obj = prism; break;
		case "PSE":
			rotation = Quaternion.Euler(0, 270.0f, 0);
			obj = prism; break;
		case "CVN":
			obj = converter; break;
		case "CVE":
			rotation = Quaternion.Euler(0, 90.0f, 0);
			obj = converter; break;
		}

		if(obj != null) {
			objects.Add(Instantiate(obj, new Vector3 (column, floor, row), rotation));
		}
	}

	// ------------------------------------------------------------------------

	void buildFloor(string type, int floor, int row, int column) {
		int pattern = 0;
		float pivotOffset = 0;

		// calculate what color pattern we need
		if (row % 2 == 0) {
			pattern = (column % 2 == 0) ? 1 : 2;
		} else {
			pattern = (column % 2 != 0) ? 1 : 2;
		}

		UnityEngine.Object obj = null;

		switch(type) {
		case "FLR":
			obj = pattern == 1 ? floorPattern1 : floorPattern2; break;
		case "BOX":
			obj = pattern == 1 ? boxPattern1 : boxPattern2;
			pivotOffset = 0.5f; break;
		case "LFD":
			obj = liftDown;
			pivotOffset = 0.5f; break;
		case "LFU":
			obj = liftUp; break;
		}

		if(obj != null) {
			objects.Add(Instantiate(obj, new Vector3 (column, floor + floorOffset + pivotOffset, row), Quaternion.identity));
		}
	}
}
