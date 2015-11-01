using UnityEngine;

public class GradientBackground : MonoBehaviour {
	public int gradientLayer = 7;
	public GameObject parent;

	private GameObject plane;
	private GameObject cam;

	// ------------------------------------------------------------------------

	private void clear() {
		if(plane) {
			Destroy(plane);
		}
		if(cam) {
			Destroy(cam);
		}
	}

	// ------------------------------------------------------------------------

	public void CreateBackground(Color32 topColor, Color32 bottomColor) {
		clear();

		gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);
   		if (!GetComponent<Camera>()) {
			return;
		}

		GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
		GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask & ~(1 << gradientLayer);

		cam = new GameObject("Cam", typeof(Camera));
		cam.transform.parent = parent.transform;
		cam.GetComponent<Camera>().cullingMask = 1 << gradientLayer;
		cam.GetComponent<Camera>().depth = GetComponent<Camera>().depth - 1;

		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[6] {
							new Vector3(-10f, .577f, 1f), // 0
							new Vector3(10f, .577f, 1f),  // 1
							new Vector3(-10f, 0f, 1f),    // 2
							new Vector3(10f, 0f, 1f),	  // 3
							new Vector3(-10f, -1f, 1f),   // 4
							new Vector3(10f, -1f, 1f)     // 5
						};

		mesh.colors32 = new Color32[6] {
			topColor, topColor, bottomColor, bottomColor, bottomColor, bottomColor
		};

		// --------------------
		// 0        1
		// 2        3
		// 4        5
		mesh.triangles = new int[12] {
			0, 1, 2,
			1, 3, 2,
			2, 3, 4,
			3, 5, 4
		};

		plane = new GameObject("Plane", typeof(MeshFilter), typeof(MeshRenderer));
		plane.layer = gradientLayer;
		plane.transform.parent = parent.transform;
		plane.GetComponent<MeshFilter>().mesh = mesh;
		plane.GetComponent<Renderer>().material = new Material("Shader \"Vertex Color Only\"{Subshader{BindChannels{Bind \"vertex\", vertex Bind \"color\", color}Pass{}}}");;
	}
}