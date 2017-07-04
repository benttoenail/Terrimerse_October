using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class dataCube : MonoBehaviour {
	public int scale = 100;
	public string filename = "";
	public string dataDirectory = "";

	public int size_x { get; protected set; }
	public int size_y { get; protected set; }
	public int size_z { get; protected set; }

	public Color colorLow = Color.Lerp(Color.red,Color.white,0.5f);
	public Color colorMid = Color.black;
	public Color colorHigh = Color.Lerp(Color.red,Color.white,0.5f);
	public float colorThreshold = 20.0f;

	public bool initialized = false;

	public float[, ,] dataArray;

	public GameObject planeContainer;

	public void Initialize() {
		readDataFile (Application.streamingAssetsPath + "/"+dataDirectory + "/" + filename);
		initialized = true;
	}

	public void Start() {
		if(!initialized) {
			readDataFile (Application.streamingAssetsPath + "/"+dataDirectory + "/" + filename);
			initialized = true;
		}
		foreach (updateplane plane in planeContainer.GetComponentsInChildren<updateplane>()) {
			plane.Initialize ();
		}
	}


	public void readDataFile(string file_path)
	{
		string[] parts = file_path.Split ('.');
		string ext = parts [parts.Length - 1];
		if (ext == "txt") {
			dataArray = VoxelImageLoader.ReadTextFile (file_path);
			string correspondingFilepath = file_path.Substring (0, file_path.Length - 4) + ".voxel";
			if (!File.Exists (correspondingFilepath)) {
				VoxelImageLoader.WriteBinaryFile (correspondingFilepath, dataArray);
			}
		} else {
			dataArray = VoxelImageLoader.ReadBinaryFile (file_path);
		}
		size_x = dataArray.GetLength (0);
		size_y = dataArray.GetLength (1);
		size_z = dataArray.GetLength (2);
	}
}
