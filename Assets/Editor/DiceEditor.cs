using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dice))]
public class DiceEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		EditorGUILayout.HelpBox("Debug Rolls", MessageType.Info);
		if (GUILayout.Button("1",GUILayout.Width(EditorGUIUtility.currentViewWidth/3), GUILayout.Height(50)))
		{
			Dice.DebugRoll(1);
		}

		if (GUILayout.Button("2",GUILayout.Width(EditorGUIUtility.currentViewWidth/3), GUILayout.Height(50)))
		{
			Dice.DebugRoll(2);
		}

		if (GUILayout.Button("3",GUILayout.Width(EditorGUIUtility.currentViewWidth/3), GUILayout.Height(50)))
		{
			Dice.DebugRoll(3);
		}

		if (GUILayout.Button("4",GUILayout.Width(EditorGUIUtility.currentViewWidth/3), GUILayout.Height(50)))
		{
			Dice.DebugRoll(4);
		}

		if (GUILayout.Button("5",GUILayout.Width(EditorGUIUtility.currentViewWidth/3), GUILayout.Height(50)))
		{
			Dice.DebugRoll(5);
		}

		if (GUILayout.Button("6",GUILayout.Width(EditorGUIUtility.currentViewWidth/3), GUILayout.Height(50)))
		{
			Dice.DebugRoll(6);
		}
	}
}