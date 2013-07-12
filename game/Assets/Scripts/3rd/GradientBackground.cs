using UnityEngine;

public class GradientBackground : MonoBehaviour {
	public int gradientLayer = 7;

	private GameObject gradientPlane;
	private GameObject gradientCam;

	// -----------------------------------------------------------------------------------------------------------------

	private void clear() {
		if(gradientPlane) {
			Destroy(gradientPlane);
		}
		if(gradientCam) {
			Destroy(gradientCam);
		}
	}

	// -----------------------------------------------------------------------------------------------------------------
	public void CreateBackground (Color topColor, Color bottomColor) {
		clear();

		gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);
   		if (!camera) {
        	Debug.LogError ("Must attach GradientBackground script to the camera");
        	return;
    	}

    	camera.clearFlags = CameraClearFlags.Depth;
    	camera.cullingMask = camera.cullingMask & ~(1 << gradientLayer);
    	gradientCam = new GameObject("Gradient Cam", typeof(Camera));
    	Camera cam = gradientCam.camera;
    	cam.depth = camera.depth-1;
    	cam.cullingMask = 1 << gradientLayer;

		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4] {
							new Vector3(-100f, .577f, 1f),
							new Vector3(100f, .577f, 1f),
							new Vector3(-100f, -0.05f, 1f),
							new Vector3(100f, -0.05f, 1f)
						};

		mesh.colors = new Color[4] {topColor,topColor,bottomColor,bottomColor};

		mesh.triangles = new int[6] {0, 1, 2, 1, 3, 2};

		Material mat = new Material("Shader \"Vertex Color Only\"{Subshader{BindChannels{Bind \"vertex\", vertex Bind \"color\", color}Pass{}}}");
    	gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

		((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
    	gradientPlane.renderer.material = mat;
    	gradientPlane.layer = gradientLayer;
	}
}