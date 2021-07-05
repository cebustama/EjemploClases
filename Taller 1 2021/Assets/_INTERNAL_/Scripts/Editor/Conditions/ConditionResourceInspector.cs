using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ConditionResource))]
public class ConditionResourceInspector : ConditionInspectorBase
{
	private string explanation = "Use this script to perform an action repeatedly.";

	private InventoryResources repository;

	private new void OnEnable()
	{
		repository = Resources.Load<InventoryResources>("ScriptableObjects/InventoryResources");

		base.OnEnable();
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		GUILayout.Space(10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		GUILayout.Space(10);
		//draw the popup that displays the names of Resource types, taken from the "InventoryResources" ScriptableObject
		SerializedProperty resourceIndexProp = serializedObject.FindProperty("checkFor");
		int chosenType = resourceIndexProp.intValue; //take the int value from the property

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Type of Resource");
		chosenType = EditorGUILayout.Popup(chosenType, repository.GetResourceTypes(), GUILayout.ExpandWidth(false));
		EditorGUILayout.EndHorizontal();

		resourceIndexProp.intValue = chosenType; //put the value back into the property

		EditorGUILayout.PropertyField(serializedObject.FindProperty("amountNeeded"));

		GUILayout.Space(10);
		//Display a button to jump to the "InventoryResources" ScriptableObject
		if (GUILayout.Button("Add/Remove types"))
		{
			Selection.activeObject = repository;
		}

		serializedObject.ApplyModifiedProperties();

		DrawActionLists();

		serializedObject.ApplyModifiedProperties();
	}
}
