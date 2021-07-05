using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ObjectShooter))]
public class ObjectShooterInspector : InspectorBase
{
	private string explanation = "Spawns an object at the press of a button and it applies a force to it in the direction chosen.";
	//private string hint = "TIP: If you want to shoot in another direction, apply this script to a child object and rotate it in the direction you want.";
	private string warning = "WARNING: Don't forget to apply a Rigidbody2D to your projectiles, or they won't move!";

	private InventoryResources repository;

	private void OnEnable()
	{
		repository = Resources.Load<InventoryResources>("ScriptableObjects/InventoryResources");
	}

	public override void OnInspectorGUI()
	{
		GUILayout.Space (10);
		EditorGUILayout.HelpBox(explanation, MessageType.Info);

		bool prefabSelected = ShowPrefabWarning("prefabToSpawn");

		if(prefabSelected)
		{
			if(!CheckIfObjectUsesComponent<Rigidbody2D>("prefabToSpawn"))
			{
				EditorGUILayout.HelpBox(warning, MessageType.Warning);
			}
		}

		GUILayout.Space(10);
		//draw the popup that displays the names of Resource types, taken from the "InventoryResources" ScriptableObject
		SerializedProperty resourceIndexProp = serializedObject.FindProperty("checkFor");
		int chosenType = resourceIndexProp.intValue; //take the int value from the property

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Type of Resource");
		chosenType = EditorGUILayout.Popup(chosenType, repository.GetResourceTypes(), GUILayout.ExpandWidth(false));
		EditorGUILayout.EndHorizontal();

		resourceIndexProp.intValue = chosenType; //put the value back into the property

		base.OnInspectorGUI();

		//removed because it's not possible to choose the direction
		//EditorGUILayout.HelpBox(hint, MessageType.Info);
	}
}
