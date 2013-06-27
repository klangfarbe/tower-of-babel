using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

// 3rd party
using LitJson;

// babel classes
using CardinalDirection;

public class LevelLoader : MonoBehaviour {
	public int level = 0;
	public float floorOffset = -0.03f;

	public GameObject allLiftsDown;
	public GameObject allLiftsUp;
	public GameObject block;
	public GameObject boxPattern1;
	public GameObject boxPattern2;
	public GameObject converter;
	public GameObject exchanger;
	public GameObject flag;
	public GameObject floorPattern1;
	public GameObject floorPattern2;
	public GameObject freezer;
	public GameObject grabber;
	public GameObject hopper;
	public GameObject klondike;
	public GameObject liftDown;
	public GameObject liftUp;
	public GameObject lizard;
	public GameObject proximityMine;
	public GameObject prism;
	public GameObject pusher;
	public GameObject pushingCannon;
	public GameObject reflector;
	public GameObject timeBomb;
	public GameObject watcher;
	public GameObject wiper;
	public GameObject worm;
	public GameObject zapper;
	public GameObject zappingCannon;

	public Material materialPattern1;
	public Material materialPattern2;

	public GUIText levelName;
	public GUIText levelNr;

	// -------------------------------------------------------------------------

	private JsonData levelData;
	private int maxColumns = 0;
	private int maxRows = 0;
	private int maxFloors = 0;

	// the current fields to build
	private int floor;
	private int row;
	private int column;

	private List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

	// -------------------------------------------------------------------------

	void Start() {
		Debug.Log("Awakening scene...");
		build();
	}

	// -------------------------------------------------------------------------

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

	// -------------------------------------------------------------------------

	public void build() {
		Debug.Log("Building level...");
		clearScene();
		loadResource();
		setMaterialColors();

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
		translateToBottomLeftCornerAsZeroZero();
		loadCameras();

	}

	// -------------------------------------------------------------------------

	void loadCameras() {
		SwitchCamera cameraScript = GetComponent<SwitchCamera>();
		cameraScript.reset();
	}

	// -------------------------------------------------------------------------

	void setLevelName() {
		levelName.text = levelData["title"].ToString();
		levelNr.text = "Level: " + level.ToString();
	}

	// -------------------------------------------------------------------------

	void translateToBottomLeftCornerAsZeroZero() {
		GameObject parent = GameObject.Find("Level");
		foreach(UnityEngine.Object o in objects) {
			((GameObject)o).transform.parent = parent.transform;
		}
		parent.transform.position = new Vector3(0, 0, maxRows);
	}

	// -------------------------------------------------------------------------

	void clearScene() {
		foreach(UnityEngine.Object o in objects) {
			Destroy(o);
		}
		objects.Clear();
		maxFloors = 0;
		maxColumns = 0;
		maxRows = 0;
		levelData = null;
		GameObject.Find("Level").transform.position = new Vector3(0, 0, 0);
	}

	// -------------------------------------------------------------------------

	void loadResource() {
		Debug.Log("Loading level data...");
		TextAsset json = Resources.Load("Level/" + string.Format("{0:d3}", level)) as TextAsset;
		levelData = JsonMapper.ToObject(json.text);
	}

	// -------------------------------------------------------------------------

	void setMaterialColors() {
		materialPattern1.color = hexToColor(levelData["fx"]["patterncolor1"].ToString().Substring(2));
		materialPattern2.color = hexToColor(levelData["fx"]["patterncolor2"].ToString().Substring(2));
		Debug.Log(materialPattern1.color.ToString());
		Debug.Log(materialPattern2.color.ToString());
	}

	// -------------------------------------------------------------------------

	Color hexToColor(string hex) {
		Debug.Log(hex);
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	// -------------------------------------------------------------------------

	void updateMaximumLevelDimensions() {
		maxFloors = floor > maxFloors ? floor : maxFloors;
		maxRows = row > maxRows ? row : maxRows;
		maxColumns = column > maxColumns ? column : maxColumns;
	}

	// -------------------------------------------------------------------------

	void calculateLevelCenter() {
		float x = maxColumns / 2f;
		float y = maxFloors / 2f;
		float z = maxRows / 2f;
		Debug.Log("Position: " + y + ", field: " + x + ", " + z);

		// Transform the camera and lights
		GameObject.Find("MainCameraParent").transform.position = new Vector3(x, y, z);
		GameObject.Find("Lights").transform.position = new Vector3(x, maxFloors + 2, z);
	}

	// -------------------------------------------------------------------------

	void buildPosition () {
		try {
			JsonData field = levelData ["elements"] [floor] [row] [column];
			Debug.Log("Building " + floor + "/" + row + "/" + column);
			updateMaximumLevelDimensions();
			buildFloor();
			buildObject();
		} catch (Exception e) {
			//Debug.Log("Field not existent " + floor + "/" + row + "/" + column);
		}
	}

	// -------------------------------------------------------------------------

	void buildObject() {
		GameObject obj = null;
		Quaternion rotation = Quaternion.identity;

		switch(getObjectTypeAt(floor, row, column)) {
			case "BFL":	obj = flag; break;
			case "BLK":	obj = block; break;
			case "EXC": obj = exchanger; break;
			case "FCD":	obj = allLiftsDown; break;
			case "FCU":	obj = allLiftsUp; break;
			case "FRZ": obj = freezer; break;
			case "GRB": obj = grabber; break;
			case "HOP": obj = hopper; break;
			case "KLD":	obj = klondike; break;
			case "PRM": obj = proximityMine; break;
			case "PSH":	obj = pusher; break;
			case "REL":	obj = reflector; break;
			case "TMB": obj = timeBomb; break;
			case "WAT": obj = watcher; break;
			case "WIP": obj = wiper; break;
			case "ZAP":	obj = zapper; break;

			case "WON": obj = worm; rotation = Quaternion.North; break;
			case "WOE": obj = worm; rotation = Quaternion.East; break;

			case "LZN": obj = lizard; rotation = Quaternion.North; break;
			case "LZE": obj = lizard; rotation = Quaternion.East; break;

			case "CVN": obj = converter; break;
			case "CVE":	obj = converter; rotation = CardinalDirection.East; break;

			case "RPN":
			case "FPN": obj = pushingCannon; rotation = CardinalDirection.North; break;
			case "FPE": obj = pushingCannon; rotation = CardinalDirection.East; break;
			case "FPS": obj = pushingCannon; rotation = CardinalDirection.South; break;
			case "FPW": obj = pushingCannon; rotation = CardinalDirection.West; break;

			case "RZN":
			case "FZN": obj = zappingCannon; rotation = CardinalDirection.North; break;
			case "FZE": obj = zappingCannon; rotation = CardinalDirection.East; break;
			case "FZS": obj = zappingCannon; rotation = CardinalDirection.South; break;
			case "FZW": obj = zappingCannon; rotation = CardinalDirection.West; break;

			case "PSW":	obj = prism; rotation = CardinalDirection.North; break;
			case "PNW": obj = prism; rotation = CardinalDirection.East; break;
			case "PNE": obj = prism; rotation = CardinalDirection.South; break;
			case "PSE": obj = prism; rotation = CardinalDirection.West; break;
		}

		if(obj != null) {
			instantiateAndTag(obj, rotation);
		}
	}

	// -------------------------------------------------------------------------

	void buildFloor() {
		int pattern = 0;

		// calculate what color pattern we need
		if (row % 2 == 0) {
			pattern = (column % 2 == 0) ? 1 : 2;
		} else {
			pattern = (column % 2 != 0) ? 1 : 2;
		}

		switch(getFloorTypeAt(floor, row, column)) {
		case "LFD": instantiateAndTag(liftDown, floorOffset); break;
		case "LFU": instantiateAndTag(liftUp, floorOffset); break;
		case "FLR":
			if(getFloorTypeAt(floor - 1, row, column) != "BOX")
				instantiateAndTag(pattern == 1 ? floorPattern1 : floorPattern2, floorOffset);
			break;
		case "BOX":
			GameObject instance = instantiateAndTag(pattern == 1 ? boxPattern1 : boxPattern2, floorOffset);
			if(getFloorTypeAt(floor + 1, row, column) == "FLR") {
				// increase box height by floorOffset
				instance.transform.localScale += new Vector3(0, -floorOffset, 0);
			}
			break;
		}
	}

	// -------------------------------------------------------------------------

	GameObject instantiateAndTag(GameObject go, float floorOffset = 0.0f) {
		return instantiateAndTag(go, Quaternion.identity, floorOffset);
	}

	// -------------------------------------------------------------------------

	GameObject instantiateAndTag(GameObject go, Quaternion rotation, float floorOffset = 0.0f) {
		if(go == null) return null;
		GameObject instance = (GameObject)Instantiate(go, new Vector3 (column, floor + floorOffset, -row), rotation);
		objects.Add(instance);
		return instance;
	}

	// -------------------------------------------------------------------------

	string getFloorTypeAt(int floor, int row, int column) {
		try {
			return levelData ["elements"] [floor] [row] [column] ["f"].ToString();
		} catch(Exception e) {
			Debug.LogException(e);
		}
		return null;
	}

	// -------------------------------------------------------------------------

	string getObjectTypeAt(int floor, int row, int column) {
		try {
			return levelData ["elements"] [floor] [row] [column] ["o"].ToString();
		} catch(Exception e) {
			Debug.LogException(e);
		}
		return null;
	}
}
