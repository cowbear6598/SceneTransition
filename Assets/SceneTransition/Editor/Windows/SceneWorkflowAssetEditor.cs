using SceneTransition.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace SceneTransition.Editor.Windows
{
	[CustomEditor(typeof(SceneWorkflowAsset))]
	public class SceneWorkflowAssetEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("_operationData"));
			EditorGUI.EndDisabledGroup();

			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.Space();

			if (GUILayout.Button("開啟編輯器"))
			{
				var window = EditorWindow.GetWindow<SceneWorkflowEditorWindow>();

				window.Load(target as SceneWorkflowAsset);
				window.Show();
			}
		}
	}
}