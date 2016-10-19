using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreatePointCloudMaterial : EditorWindow {

	GameObject[] selected;

	//Show the custom script in window
	[MenuItem ("Window/CustomScripts/Create Point Cloud Material", false, 2)]

	//Show the window
	public static void ShowWindow() {

		EditorWindow.GetWindow(typeof(CreatePointCloudMaterial));

	}

	//Creates a GUI
	void OnGUI() {

		//Create Materials for selected objects
		GUILayout.Label("Create Materials", EditorStyles.boldLabel);
		if(GUILayout.Button("Create", GUILayout.Height(40))){
			CreateMaterials();
		}

		//Delete all Materials / shaders for selected objects
		GUILayout.Label("Delete Materials", EditorStyles.boldLabel);
		if(GUILayout.Button("Delete", GUILayout.Height(40))){
			RemoveShaders();
		}

		//Create sprite for all vertex points
		GUILayout.Label("Create Sprite", EditorStyles.boldLabel);
		if(GUILayout.Button("Create Sprites", GUILayout.Height(40))){
			SpirteToVertex();
		}

	}

	//Create a new material for each selected item 
	void CreateMaterials() {

		Debug.Log("Creating Materials");

		Debug.Log(Selection.gameObjects.Length);

		for(int i = 0; i < Selection.gameObjects.Length; i++){
			
			MeshRenderer rend = Selection.gameObjects[i].GetComponent<MeshRenderer>();
			Material mat = new Material(Shader.Find("GUI/Text Shader"));

			float red = Random.Range(0.2f, 1.0f);
			float green = Random.Range(0.2f, 1.0f);
			float blue = Random.Range(0.2f, 1.0f);

			Color color = new Color(red, green, blue);

			mat.color = color;
			rend.material = mat;

		}

	}

	//Delete all shaders
	void RemoveShaders() {

		Debug.Log("Removing all shaders");

		for(int i = 0; i < Selection.gameObjects.Length; i++){
			MeshRenderer rend = Selection.gameObjects[i].GetComponent<MeshRenderer>();
			DestroyImmediate(rend.sharedMaterial);
		}

	}


	void SpirteToVertex() {

		Mesh mesh = Selection.gameObjects[0].GetComponent<MeshFilter>().sharedMesh;
		Vector3[] verticies = mesh.vertices;

		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		plane.transform.position = verticies[0];

		Debug.Log(verticies[0]);

	}
}
