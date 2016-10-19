// Point Cloud Binary Viewer DX11
// reads custom binary file and displays it
// http://unitycoder.com

using UnityEngine;
using System.Collections;
//using System.IO;
//using System.BitConverter;
using System;

namespace unitycodercom_PointCloudWebplayerViewer
{
	public class WebplayerViewerDX11 : MonoBehaviour 
	{
		//public string baseFolder = "data/";
		public string URL = "http://unitycoder.com/upload/demos/PointCloudViewerDX11/web1/sample.bin";
//		private string URL = "file:///"+""+"/sample.bin";
		public Material material;
		private bool readRGB = false;
		private int totalPoints=0;
		private ComputeBuffer bufferPoints;
		private ComputeBuffer bufferPos;
		private ComputeBuffer bufferColors;
		private int instanceCount = 1;
//		private Vector3[] origPos;
		private Vector3[] pos;
		private Vector3[] points;
		private Vector3[] colors;
		private WWW www;
		private bool loading = false;
		private GameObject loadStatus;

//		private bool stream = true;
//		private bool streamReady = false;



		// start reading file at Start
		void Start () 
		{
			// start reading pointcloud & display it
			StartCoroutine("ReadPointCloud");
			
		}
		

		void Update()
		{
			if (loading)
			{

				// stream not possible with default www ?
//				if (stream)
//				{
//					if (!streamReady && totalPoints>0)
//					{
//						streamReady = true;
//						Debug.Log("streamready:"+totalPoints);
//					}
//				}


				loadStatus.GetComponent<GUIText>().text = "Loading: "+(int)(www.progress*100)+"%";// ("+www.bytesDownloaded+" b)";
				return;
			}

			return;

		}


		
		// binary point cloud reader
		IEnumerator ReadPointCloud()
		{
			//URL = "file:///"+Application.dataPath+"/sample.bin";

			//Debug.Log(URL);

			// use WWW for webplayer
			www = new WWW(URL);
			loadStatus = new GameObject();
			loadStatus.name = "go_progress_temp";
			loadStatus.AddComponent<GUIText>();
			loadStatus.transform.position = new Vector3(0.5f,1,100);
			loading = true;
			yield return www;

			if (!String.IsNullOrEmpty(www.error))
			{
				Debug.LogError(www.error);
			}

			//Debug.Log("downloaded:" + www.bytesDownloaded);


			loading = false;
			byte[] reader = www.bytes;

			// get version number
//			byte binaryVersion = reader[0]; // not used

			// get total points
			totalPoints = System.BitConverter.ToInt32(reader, 1);

			//Debug.Log (totalPoints);
			Destroy(loadStatus.gameObject);

			if (totalPoints<1)
			{
				Debug.LogError ("Error: No points founded");
			}

			// get boolean
			readRGB = System.BitConverter.ToBoolean(reader, 5);

			// init arrays
			points = new Vector3[totalPoints];
			
			if (readRGB)
			{
				colors = new Vector3[totalPoints];
			}

			int counter = 6;

			if (readRGB)
			{

				for (int n=0;n<totalPoints-1;n++)
				{
					float x1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					float y1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					float z1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					float r1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					float g1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					float b1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					points[n] = new Vector3(x1,y1,z1)+transform.position;
					colors[n] = new Vector3(r1,g1,b1);
				} 
				
				
			}else{
				for (int n=0;n<totalPoints-1;n++)
				{
					float x1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					float y1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					float z1 = System.BitConverter.ToSingle(reader, counter);
					counter+=4;
					points[n] = new Vector3(x1,y1,z1)+transform.position;
				} 
			}

			// init dx11 buffers
			ReleaseBuffers ();
			bufferPoints = new ComputeBuffer (totalPoints, 12);
			bufferPoints.SetData (points);
			material.SetBuffer ("buf_Points", bufferPoints);
			bufferPos = new ComputeBuffer (instanceCount, 12);
			material.SetBuffer ("buf_Positions", bufferPos);
			
			if (readRGB)
			{
				bufferColors = new ComputeBuffer (totalPoints, 12);
				bufferColors.SetData (colors);
				material.SetBuffer ("buf_Colors", bufferColors);
			}
			
//			pos = new Vector3[instanceCount];
//			pos[0] = new Vector3(0.0f,0,0);
//			bufferPos.SetData (pos);
//			bufferPoints.SetData (points);

			Vector3[] pos = new Vector3[instanceCount];
			pos[0] = new Vector3(0.0f,0,0);
			bufferPos.SetData (pos);
			
			// cleanup temp arrays
			//Array.Clear(pos,0,pos.Length);
			//Array.Clear(points,0,points.Length); // keep for selection
			//Array.Clear(colors,0,colors.Length);

			
		} // readpointcloud
		


		void OnDestroy()
		{
			if( www != null) { www.Dispose(); }

			ReleaseBuffers ();

		}



		// DX11 stuff
		void ReleaseBuffers() 
		{
			if (bufferPoints != null) bufferPoints.Release();
			bufferPoints = null;
			if (bufferPos != null) bufferPos.Release();
			bufferPos = null;
			if (bufferColors != null) bufferColors.Release();
			bufferColors = null;
		}
		void OnDisable() {
			ReleaseBuffers ();
		}
		
		
		// mainloop, for displaying the points
		//	void OnPostRender () // this works also
		void OnRenderObject () 
		{
			material.SetPass (0);
			Graphics.DrawProcedural (MeshTopology.Points , totalPoints, instanceCount);
		}
		
	} // class
} // namespace