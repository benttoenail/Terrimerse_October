using UnityEngine;
using System.Collections;

public class updateplane : MonoBehaviour {

	// Public Vars:
	public dataCube data;
	public int sliceType = 2; // xy=1, xz=2, yz=3

	// Private Vars:
	private Vector3 initPosition;
	private Vector3 previousPosition;
	private GameObject backside;

	private Texture2D mainTexture;

	public bool wasMoved = false;

	bool initialized = false;

	// Use this for initialization
	public void Initialize () {
		
		//Set size of plane:
		setSize(data, sliceType);
		//Set position of plane:
		setInitialPosition ();

		//Get initial texture on plane
		mainTexture = makeTexture (getPlaneWidth (), getPlaneHeight ());
		setDataPixels (mainTexture.width, mainTexture.height, 0, data.dataArray, mainTexture);
		applyTexture (mainTexture);

		//Initialize Plane Position
		initPosition = transform.localPosition;
		previousPosition = initPosition;

		// Initialize backside
		initBackside ();

		initialized = true;
	}

	void initBackside(){
		backside = transform.GetChild (0).gameObject;
		backside.transform.localScale = new Vector3 (1, -1, 1);
		refreshBacksideTexture ();
	}

	void refreshBacksideTexture() {
		backside.GetComponent<Renderer> ().material.mainTexture = GetComponent<Renderer> ().material.mainTexture;
	}


	// Get plane position index (uses FLOOR - consider using ROUND) 
	int getPlanePosition() {
		Vector3 thisPosition = transform.localPosition - initPosition;
		switch (sliceType)
		{
		case 1:
			return (int)Mathf.Floor(thisPosition.z * data.scale / 10);
		case 2:
			return (int)Mathf.Floor(thisPosition.y * data.scale / 10);
		case 3:
			return (int)Mathf.Floor(thisPosition.x * data.scale / 10);
		default:
			Debug.Log ("Error getting plane position");
			return 0;
		}
	}

	void setInitialPosition() {
		switch (sliceType)
		{
		case 1:
			transform.localPosition = new Vector3 (0, 0, -(float)data.size_z / data.scale * 10 / 2);
			//Debug.Log ((float)data.scale / data.scale * 10 / 2);
			break;
		case 2:
			transform.localPosition = new Vector3 (0, -(float)data.size_y / data.scale * 10 / 2, 0);
			break;
		case 3:
			transform.localPosition = new Vector3 (-(float)data.size_x / data.scale * 10 / 2, 0, 0);
			break;
		default:
			Debug.Log ("Error setting initial position");
			break;
		}
	}



	/// <summary>
	/// Gets a texture from a position.
	/// </summary>
	/// <returns>The texture.</returns>
	/// <param name="w">The width of the plane.</param>
	/// <param name="h">The height of the plane.</param>
	/// <param name="pos">The position of the plane (3rd coordinate).</param>
	/// <param name="data">3D Cube data array.</param>
	/// <param name="texture">Preexisting texture to overwrite (optional).</param>
	static Texture2D makeTexture(int w, int h) {
		Texture2D texture = new Texture2D (w, h, TextureFormat.RGBA32, false);

		return texture;
	}

	void setDataPixels(int w, int h, int pos, float[, ,] dataValues, Texture2D texture) {
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				switch (sliceType)
				{
				case 1:
					texture.SetPixel (i, j, getColor (dataValues [i, j, pos]));
					break;
				case 2:
					texture.SetPixel (i, j, getColor(dataValues [i, pos, j]));
					break;
				case 3:
					texture.SetPixel (i, j, getColor(dataValues [pos, j, i]));
					break;
				default:
					Debug.Log ("Error getting texture");
					texture.SetPixel (i, j, new Color(255,0,0));
					break;
				}
			}
		}
		texture.Apply ();
	}

	// Applies a given texture to plane
	void applyTexture(Texture2D texture) {
		GetComponent<Renderer> ().material.mainTexture = texture;
	}

	// Get width of plane realtive to planeType
	int getPlaneWidth() {
		switch (sliceType)
		{
		case 1:
			return data.size_x;
		case 2:
			return data.size_x;
		case 3:
			return data.size_z;
		default:
			Debug.Log ("Error getting plane width");
			return 0;
		}
	}

	// Get height of plane relative to planeType
	int getPlaneHeight() {
		switch (sliceType)
		{
		case 1:
			return data.size_y;
		case 2:
			return data.size_z;
		case 3:
			return data.size_y;
		default:
			Debug.Log ("Error getting plane height");
			return 0;
		}
	}

	// Get color from float value:
	Color getColor(float value) {
		//return Color.HSVToRGB (value / 50 + 0.5f, 1, 1);
		if (value < 0) {
			return Color.Lerp (data.colorMid, data.colorLow, -value/data.colorThreshold);
		} else {
			return Color.Lerp (data.colorMid, data.colorHigh, value/data.colorThreshold);
		}

		//return new Color(value/25,0,-value/25);
		//return new Color((value+25)/50,0,0);
	}

	//Set size of plane according to type:
	void setSize(dataCube data, int axis) {
		switch (axis)
		{
		case 1:
			transform.localScale = new Vector3 (-(float)data.size_x / data.scale, 1, (float)data.size_y / data.scale);
			transform.localRotation = Quaternion.Euler (90, 0, 0);
			break;
		case 2:
			transform.localScale = new Vector3((float)data.size_x/data.scale,1,(float)data.size_z/data.scale);
			transform.localRotation = Quaternion.Euler (0, 180, 0);
			break;
		case 3:
			transform.localScale = new Vector3((float)data.size_z/data.scale,1,(float)data.size_y/data.scale);
			transform.localRotation = Quaternion.Euler (90, 90, 0);
			break;
		default:
			Debug.Log("size was not set, axis not valid");
			break;
		}
	}

	// Determines if plane is within scope of data
	bool withinRange() {
		switch (sliceType)
		{
		case 1:
			if (getPlanePosition () >= data.size_z) {
				transform.localPosition = initPosition + new Vector3(0,0,data.size_z / 100f * 10f);
				return false;
			}
			if (getPlanePosition () < 0) {
				transform.localPosition = initPosition;
				return false;
			}
			return true;
		case 2:
			if (getPlanePosition () >= data.size_y) {
				transform.localPosition = initPosition + new Vector3(0,data.size_y / 100f * 10f,0);
				return false;
			}
			if (getPlanePosition () < 0) {
				transform.localPosition = initPosition;
				return false;
			}
			return true;
		case 3:
			if (getPlanePosition () >= data.size_x) {
				transform.localPosition = initPosition + new Vector3(data.size_x / 100f * 10f,0,0);
				return false;
			}
			if (getPlanePosition () < 0) {
				transform.localPosition = initPosition;
				return false;
			}
			return true;
		default:
			Debug.Log ("Error determining range for plane");
			return false;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (!initialized) {
			return;
		}

		if (wasMoved && withinRange() && data.initialized) {
			if (mainTexture == null) {
				mainTexture = makeTexture (getPlaneWidth (), getPlaneHeight ());
				applyTexture (mainTexture);
				refreshBacksideTexture ();
			}

			setDataPixels(mainTexture.width, mainTexture.height, getPlanePosition(), data.dataArray, mainTexture);

			// Update Position:
			previousPosition = transform.localPosition;
			wasMoved = false;
		}

	}







}
