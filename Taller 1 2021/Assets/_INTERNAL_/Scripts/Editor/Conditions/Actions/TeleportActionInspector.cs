using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(TeleportAction))]
public class TeleportActionInspector : InspectorBase
{
	private string explanation = "Use this script to teleport this or another object to a new location.";
	private string objectWarning = "WARNING: If you don't assign a GameObject, this GameObject will be teleported!";

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		GUILayout.Space(10);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("objectToMove"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("objectThatCollided"));

		if (!CheckIfAssigned("objectToMove", false))
		{
			//EditorGUILayout.HelpBox(objectWarning, MessageType.Warning);
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("newPositionObject"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("newPosition"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("stopMovements"));

		if (GUI.changed)
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
