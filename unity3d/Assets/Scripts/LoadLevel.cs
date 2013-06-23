using UnityEngine;
using System.Collections.Generic;

using LitJson;
using System;
using System.IO;

public class LoadLevel : MonoBehaviour {
	public int level = 0;
	public float floorOffset = -0.03f;
	public GameObject grabber;
	public GameObject pusher;
	public GameObject zapper;
	public GameObject floorPattern1;
	public GameObject floorPattern2;
	public GameObject boxPattern1;
	public GameObject boxPattern2;
	public GameObject liftUp;
	public GameObject liftDown;
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
	
	// the current fields to build
	private int floor;
	private int row;
	private int column;

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
	}

	// ------------------------------------------------------------------------

	void setLevelName() {
		levelName.text = levelData["title"].ToString();
		levelNr.text = "Level: " + level.ToString();
	}

	// ------------------------------------------------------------------------
	
	void translateToBottomLeftCornerAsZeroZero() {
		GameObject parent = GameObject.Find("Level");
		foreach(UnityEngine.Object o in objects) {
			((GameObject)o).transform.parent = parent.transform;
		}
		parent.transform.position = new Vector3(0, 0, maxRows);
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
		GameObject.Find("Level").transform.position = new Vector3(0, 0, 0);
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

	void updateMaximumLevelDimensions() {
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
	
	void buildPosition () {
		try {
			JsonData field = levelData ["elements"] [floor] [row] [column];
			Debug.Log("Building " + floor + "/" + row + "/" + column);
			updateMaximumLevelDimensions();
			buildFloor();
			buildObject();
		} catch (Exception e) {
			Debug.Log("Field not existent " + floor + "/" + row + "/" + column);
		}
	}

	// ------------------------------------------------------------------------

	void buildObject() {
		GameObject obj = null;
		Quaternion rotation = Quaternion.identity;
		
		switch(getObjectTypeAt(floor, row, column)) {
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
			instantiateAndTag(obj, rotation);
		}
	}
	
	// ------------------------------------------------------------------------

	void buildFloor() {
		int pattern = 0;
		
		// calculate what color pattern we need
		if (row % 2 == 0) {
			pattern = (column % 2 == 0) ? 1 : 2;
		} else {
			pattern = (column % 2 != 0) ? 1 : 2;
		}

		switch(getFloorTypeAt(floor, row, column)) {
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
		case "LFD":
			instantiateAndTag(liftDown, floorOffset); 
			break;
		case "LFU":
			instantiateAndTag(liftUp, floorOffset); 
			break;
		}
	}
		
	// ------------------------------------------------------------------------
	
	GameObject instantiateAndTag(GameObject go, float floorOffset = 0.0f) {
		return instantiateAndTag(go, Quaternion.identity, floorOffset);
	}
	
	// ------------------------------------------------------------------------
	
	GameObject instantiateAndTag(GameObject go, Quaternion rotation, float floorOffset = 0.0f) {
		if(go == null)
			return null;
		
		GameObject instance = (GameObject)Instantiate(go, 
			new Vector3 (column, floor + floorOffset, -row), 
			rotation == null ? Quaternion.identity : rotation);
		//instance.tag = "";
		
		objects.Add(instance);
		return instance;
	}
	
	// ------------------------------------------------------------------------
	
	string getFloorTypeAt(int floor, int row, int column) {
		try {
			return levelData ["elements"] [floor] [row] [column] ["f"].ToString();
		} catch(Exception e) {
			Debug.LogException(e);
		}	
		return null;
	}
		
	// ------------------------------------------------------------------------
	
	string getObjectTypeAt(int floor, int row, int column) {
		try {
			return levelData ["elements"] [floor] [row] [column] ["o"].ToString();
		} catch(Exception e) {
			Debug.LogException(e);
		}	
		return null;
	}
}
