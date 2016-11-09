using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class dataCube : MonoBehaviour {

	public int size_x = 400; //50
	public int size_y = 221; //20
	public int size_z = 271; //30

	public int scale = 100;
	public string filename = "";
	public string dataDirectory = "";

	public bool initialized = false;

	public float[, ,] dataArray;

	public void initialize() {
		dataArray = new float[size_x, size_y, size_z];
		readDataFile (Application.dataPath + "/"+dataDirectory + "/" + filename, dataArray);
		initialized = true;
	}


	public void readDataFile(string file_path, float[, ,] dataArray)
	{
		StreamReader inStream = new StreamReader(file_path);

		while(!inStream.EndOfStream)
		{
			string line = inStream.ReadLine( );
			string[] data = line.Split (' ');
			//if (int.Parse(data [0]) == 0) {
				//dataArray [int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2])] = -25;
				//if (int.Parse (data [1]) == 19) {
				//	dataArray [int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2])] = 25;
				//	if(int.Parse(data [2]) == 0) {
				//		dataArray [int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2])] = 99;
				//	}
				//}

			//} else {
				dataArray [int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2])] = float.Parse (data [3]);
			//}

		}
		inStream.Close( );
	}
}
