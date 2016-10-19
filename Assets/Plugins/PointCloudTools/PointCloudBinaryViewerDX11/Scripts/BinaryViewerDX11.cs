// Point Cloud Binary Viewer DX11
// reads custom binary file and displays it
// http://unitycoder.com

#if !UNITY_WEBPLAYER

using UnityEngine;
using System.Collections;
using System.IO;

namespace unitycodercom_PointCloudBinaryViewer
{
	//[ExecuteInEditMode] // ** You can enable this, if you want to see DX11 cloud inside editor, without playmode **
	public class BinaryViewerDX11 : MonoBehaviour 
	{
		public string baseFolder = "data/";
		public string fileName = "out.bin";
		public Material cloudMaterial;

		[Tooltip("Brekel Binary Animated Frames")]
		public bool isAnimated = false; // Brekel binary cloud frames
		public float playbackDelay= 0.025F;


		private byte binaryVersion=0;
		private int numberOfFrames=0;
		//private float frameRate=30f; // Not used yet
		private bool containsRGB = false;

		// Brekel animated frames variables
		private int[] numberOfPointsPerFrame;
		private System.Int64[] frameBinaryPositionForEachFrame;
		private Vector3[] hugePointArray;
		private Vector3[] hugeColorArray;
		private int totalNumberOfPoints; // total from each frame
		private int currentFrame=0;
		private int[] hugeOffset;
		private float nextFrame = 0.0F;
	
		//private bool readNormals = false;
		private int totalPoints=0;
		private ComputeBuffer bufferPoints;
//		private ComputeBuffer bufferPos;
		private ComputeBuffer bufferColors;
		private const int instanceCount = 1;
		private Vector3[] points;
		private Vector3[] pointColors;
		private string fileToRead = "";
		private Vector3 dataColor;
		private float r,g,b;

		private bool isLoading = false;


		void Start() 
		{
			fileToRead= Application.dataPath+"/"+baseFolder+fileName; // get full path to file
			//fileToRead = Application.streamingAssetsPath + "/" + fileName;
			ReadPointCloud();
		}


		void Update()
		{
			if (isLoading) return;

			// For reload testing (could change the cloud filename to another and then load)
//			if (Input.GetKeyDown("r"))
//			{
//				ReleaseBuffers();
//				ReadPointCloud();
//			}

			if (isAnimated) // brekel animated cloud
			{
				if (Time.time > nextFrame) 
				{
					nextFrame = Time.time + playbackDelay;
					System.Array.Copy(hugePointArray,hugeOffset[currentFrame],points,0,numberOfPointsPerFrame[currentFrame]);
					System.Array.Copy(hugeColorArray,hugeOffset[currentFrame],pointColors,0,numberOfPointsPerFrame[currentFrame]);
					bufferPoints.SetData(points);
					bufferColors.SetData(pointColors);
					currentFrame=(++currentFrame) % (numberOfFrames);
				}
			}

		}

		void ShowMessage(string msg)
		{
			GameObject go = new GameObject ();
			go.transform.position = new Vector3 (0, 0.5f, 0);
			go.AddComponent<GUIText> ();
			go.GetComponent<GUIText>().text = msg;
		}

		// binary point cloud reader
		void ReadPointCloud()
		{
			if (!CheckIfFileExists(fileToRead))
			{
				Debug.LogError("File not found:" + fileToRead);
				ShowMessage("File not found: " + fileToRead);
				return;
			}

			if (isAnimated)
			{
				ReadAnimatedPointCloud();
				return;
			}

			isLoading = true;


			// for testing loading times
//			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//			stopwatch.Start();

			// new loader
			var data = File.ReadAllBytes(fileToRead);


			System.Int32 byteIndex = 0;

			int binaryVersion = data[byteIndex]; // Not used here
			byteIndex +=sizeof(System.Byte);
			if (binaryVersion>1) {Debug.LogError("File binaryVersion should have value (0) or (1). Loading cancelled."); return;}

			totalPoints = (int)System.BitConverter.ToInt32(data,byteIndex);
			byteIndex +=sizeof(System.Int32);

			containsRGB = System.BitConverter.ToBoolean(data,byteIndex);
			byteIndex +=sizeof(System.Boolean);

			points = new Vector3[totalPoints];

			Debug.Log("Loading "+totalPoints+" points..");

			float x,y,z;
			Vector3 dataPoint=Vector3.zero;

			if (containsRGB) pointColors = new Vector3[totalPoints];

			for(int i=0;i<totalPoints;i++)
			{
				x = System.BitConverter.ToSingle(data,byteIndex);

				byteIndex +=sizeof(float);
				y = System.BitConverter.ToSingle(data,byteIndex);
				byteIndex +=sizeof(float);
				z = System.BitConverter.ToSingle(data,byteIndex);
				byteIndex +=sizeof(float);

				dataPoint.Set(x,y,z);
				points[i] = dataPoint+transform.position;

				if (containsRGB)
				{
					r = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex +=sizeof(System.Single);
					g = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex +=sizeof(System.Single);
					b = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex +=sizeof(System.Single);

					dataColor.Set (r,g,b);
					pointColors[i] = dataColor;
				}
			} 



			/*
			// OLD READER, reads from file, 1 valua at a time, slower, but uses less memory?

			var reader = new BinaryReader(File.OpenRead(fileToRead));

			// get version number
			//byte binaryVersion = 
			reader.ReadByte(); // format version number
			// get total points

			totalPoints = reader.ReadInt32();

			// we have RGB data?
			containsRGB = reader.ReadBoolean();
//			readNormals = reader.ReadBoolean();
			//Debug.Log ("totalPoints:"+totalPoints);

			// init arrays
			points = new Vector3[totalPoints];
			
			if (containsRGB)
			{
				pointColors = new Vector3[totalPoints];
			}

			int counter = 0;
			if (containsRGB)
			{
				while (reader.BaseStream.Position != reader.BaseStream.Length)
				{
					// X Y Z R G B (float)
					// transform position is added as offset, but should keep it at 0,0,0 normally
					points[counter] = new Vector3(reader.ReadSingle()+transform.position.x,reader.ReadSingle()+transform.position.y,reader.ReadSingle()+transform.position.z);
					pointColors[counter] = new Vector3(reader.ReadSingle(),reader.ReadSingle(),reader.ReadSingle());
					counter++;
				} 

			}else{ // no rgb
				
				while (reader.BaseStream.Position != reader.BaseStream.Length)
				{
					// X Y Z
					points[counter] = new Vector3(reader.ReadSingle(),reader.ReadSingle(),reader.ReadSingle());
					counter++;
				} 
			}
			reader.Close();
*/

			// for testing load timer
//			stopwatch.Stop();
//			Debug.Log("Timer: " + stopwatch.ElapsedMilliseconds+"ms"); // this one gives you the time in ms
//			stopwatch.Reset();

			InitDX11Buffers();

			isLoading = false;

		} 



		// For Brekel animated binary data only
		void ReadAnimatedPointCloud()
		{

			if (!isAnimated)
			{
				Debug.LogWarning("ReadAnimatedPointCloud() called, but isAnimated = false");
				return;
			}

			isLoading = true;

			// NOTE: Reads whole file into memory, could cause problems with huge files
			// TODO: Add option to stream from disk or read in smaller chunks
			var data = File.ReadAllBytes(fileToRead);

			if (data.Length<1)
			{
				Debug.LogError("ReadAnimatedPointCloud() called, but isAnimated = false");
				return;
			}

			System.Int32 byteIndex = 0;
			binaryVersion = data[byteIndex];
			if (binaryVersion!=2) {Debug.LogError("File binaryVersion should have value (2) or bigger. Loading cancelled."); isAnimated = false; return;}
			byteIndex +=sizeof(System.Byte);
			numberOfFrames = (int)System.BitConverter.ToInt32(data,byteIndex);
			byteIndex +=sizeof(System.Int32);
//			frameRate = System.BitConverter.ToSingle(data,byteIndex); // not used yet
			byteIndex +=sizeof(System.Single);
			containsRGB = System.BitConverter.ToBoolean(data,byteIndex);
			byteIndex +=sizeof(System.Boolean);

			numberOfPointsPerFrame = new int[numberOfFrames];

			totalPoints=0;
			for (int i=0;i<numberOfFrames;i++)
			{
				numberOfPointsPerFrame[i] = (int)System.BitConverter.ToInt32(data,byteIndex);
				//Debug.Log(numberOfPointsPerFrame[i]);
				byteIndex +=sizeof(System.Int32);
				if (numberOfPointsPerFrame[i]>totalPoints) totalPoints = numberOfPointsPerFrame[i]; // largest value will be used as a fixed size for point array
				totalNumberOfPoints+=numberOfPointsPerFrame[i];
			}
			//return;

			hugePointArray = new Vector3[totalNumberOfPoints];
			hugeColorArray = new Vector3[totalNumberOfPoints];


			frameBinaryPositionForEachFrame = new System.Int64[numberOfFrames];
			for (int i=0;i<numberOfFrames;i++)
			{
				frameBinaryPositionForEachFrame[i] = (System.Int64)System.BitConverter.ToInt64(data,byteIndex);
				byteIndex +=sizeof(System.Int64);
			}

			/*
			Debug.Log("binaryVersion:"+binaryVersion);
			Debug.Log("numberOfFrames:"+numberOfFrames);
			Debug.Log("frameRate:"+frameRate);
			Debug.Log("containsRGB:"+containsRGB);
			Debug.Log("numberOfPointsPerFrame[0]:"+numberOfPointsPerFrame[0]);
			Debug.Log("frameBinaryPositionForEachFrame[0]:"+frameBinaryPositionForEachFrame[0]);
			*/

			points = new Vector3[totalPoints];
			hugeOffset = new int[numberOfFrames];
			int totalCounter = 0;

			if (containsRGB) pointColors = new Vector3[totalPoints];
			for (int frame=0;frame<numberOfFrames;frame++)
			{
				hugeOffset[frame] = totalCounter;
				for (int i=0;i<numberOfPointsPerFrame[frame];i++)
				{
					// X Y Z R G B (float)

					float x = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex+=sizeof(System.Single);

					float y = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex+=sizeof(System.Single);
					float z = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex+=sizeof(System.Single);

					float r = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex+=sizeof(System.Single);
					float g = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex+=sizeof(System.Single);
					float b = System.BitConverter.ToSingle(data,byteIndex);
					byteIndex+=sizeof(System.Single);

					hugePointArray[totalCounter] = new Vector3(x+transform.position.x,y+transform.position.y,z+transform.position.z);
					if (containsRGB) hugeColorArray[totalCounter] = new Vector3(r,g,b);

					totalCounter++;
				} 
			}

			InitDX11Buffers();

			isLoading = false;

		} 

		void InitDX11Buffers()
		{
			// init dx11 buffers
			ReleaseDX11Buffers();
			bufferPoints = new ComputeBuffer(totalPoints, 12);
			bufferPoints.SetData(points);
			cloudMaterial.SetBuffer("buf_Points", bufferPoints);
			//bufferPos = new ComputeBuffer (instanceCount, 12);
			//cloudMaterial.SetBuffer("buf_Positions", bufferPos);
			if (containsRGB)
			{
				bufferColors = new ComputeBuffer (totalPoints, 12);
				bufferColors.SetData(pointColors);
				cloudMaterial.SetBuffer("buf_Colors", bufferColors);
			}

//			Vector3[] pos = new Vector3[instanceCount];
//			pos[0] = new Vector3(0.0f,0,0);
//			bufferPos.SetData (pos);
		}
		



		bool CheckIfFileExists (string fileToRead)
		{
			//if (!File.Exists (fileToRead) && !fileToRead.Contains(".bin"))  fileToRead+=".bin"; // try with extension


			if (!File.Exists (fileToRead)) 
			{
				return false;
			}
			return true;
		}


		void ReleaseDX11Buffers() 
		{
			if (bufferPoints != null) bufferPoints.Release();
			bufferPoints = null;
//			if (bufferPos != null) bufferPos.Release();
//			bufferPos = null;
			if (bufferColors != null) bufferColors.Release();
			bufferColors = null;
		}


		void OnDisable() 
		{
			ReleaseDX11Buffers();

			points = new Vector3[0];
			pointColors = new Vector3[0];

			
		}


		// mainloop, for displaying the points
		//	void OnPostRender () // < works also
		void OnRenderObject () 
		{
			cloudMaterial.SetPass (0);
			Graphics.DrawProcedural (MeshTopology.Points , totalPoints, instanceCount);
		}


	} // class
} // namespace

#endif