using System;
using System.Collections;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrazyCaveLevelManager))]
[CanEditMultipleObjects]
public class EditorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			GameObject.Find("LevelManager").GetComponent<CrazyCaveLevelManager>().Generate();
		}

		if (GUILayout.Button("Clear map"))
		{
			GameObject.Find("LevelManager").GetComponent<CrazyCaveLevelManager>().ClearMap();
		}
	}
}