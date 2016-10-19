using UnityEngine;
using System.Collections;

namespace unitycoder_extras
{

	public class VideoDepthPlayer: MonoBehaviour 
	{
		public bool usePoints = false;

		void Start() 
		{
			PlayMovie();
		}

		void PlayMovie()
		{
			if (usePoints)
			{
				Mesh mesh = GetComponent<MeshFilter>().mesh;
				int[] tris = mesh.triangles;
				mesh.SetIndices (tris, MeshTopology.Points, 0);
			}

			//for now we use 2 separate videos, 1 for color, 1 for depth (grayscale)
			var r = GetComponent<Renderer>();
			MovieTexture mainTex = r.material.mainTexture as MovieTexture;
			MovieTexture depthTex = r.material.GetTexture("_ExtrudeTex") as MovieTexture;

			if (mainTex==null || depthTex==null)
			{
				Debug.LogError("Both textures should be movie textures", gameObject);
				return;
			}

			mainTex.loop = true;
			depthTex.loop = true;
			
			mainTex.Play();
			depthTex.Play();
		}
	}
}