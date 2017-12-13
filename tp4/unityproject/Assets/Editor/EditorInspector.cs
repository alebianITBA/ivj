using System;
using System.Collections;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrazyCaveLevelManager))]
[CanEditMultipleObjects]
public class EditorInspector : Editor {
	private bool generated = false;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if (GUILayout.Button("Generate"))
		{
			if (generated) {
				CrazyCaveLevelManager.Instance.ClearMap ();
			}
			CrazyCaveLevelManager.Instance.Generate ();
			generated = true;
		}

		if (GUILayout.Button("Clear map"))
		{
			GameObject.Find("LevelManager").GetComponent<CrazyCaveLevelManager>().ClearMap();
			generated = false;
		}
	}
}