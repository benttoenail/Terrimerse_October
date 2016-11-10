using UnityEngine;
using System.Collections;
using System.IO;
using Google.Protobuf;
using System.Runtime.Serialization.Formatters.Binary;

public static class VoxelImageLoader {

	public static float [ , , ] ReadTextFile(string filename) {
		float[,,] data;

		// Determine array size
		using (StreamReader streamReader = new StreamReader (filename)) {
			string lastLine = "";
			while (!streamReader.EndOfStream) {
				lastLine = streamReader.ReadLine( );
			}
			string[] split = lastLine.Split (' ');
			data = new float[int.Parse (split [0])+1, int.Parse (split [1])+1, int.Parse (split [2])+1];
		}

		// Read values
		using (StreamReader streamReader = new StreamReader (filename)) {
			while (!streamReader.EndOfStream) {
				string line = streamReader.ReadLine ();
				string[] lineData = line.Split (' ');
				data [int.Parse (lineData [0]), int.Parse (lineData [1]), int.Parse (lineData [2])] = float.Parse (lineData [3]);
			}
		}

		return data;
	}

	public static float [ , , ] ReadBinaryFile(string filename) {
		using(FileStream fileStream = new FileStream(filename,FileMode.Open,FileAccess.Read)) {
			BinaryFormatter bf = new BinaryFormatter ();
			return (float[,,]) bf.Deserialize (fileStream);
		}
	}

	public static void WriteBinaryFile(string filename, float [, , ] data) {
		using(FileStream fileStream = new FileStream(filename,FileMode.Create,FileAccess.Write)) {
			BinaryFormatter bf = new BinaryFormatter ();
			bf.Serialize (fileStream,data);
		}
	}
}
