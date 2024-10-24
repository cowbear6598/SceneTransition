using SceneTransition.ScriptableObjects;
using UnityEditor;

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
		}
	}
}