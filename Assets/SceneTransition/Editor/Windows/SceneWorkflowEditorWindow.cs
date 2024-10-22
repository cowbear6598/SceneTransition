using SceneTransition.Editor.GraphViews;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.Windows
{
	public class SceneWorkflowEditorWindow : EditorWindow
	{
		private SceneWorkflowGraphView _graphView;

		[MenuItem("SceneTransition/Workflow Editor")]
		public static void OpenWindow()
		{
			var window = GetWindow<SceneWorkflowEditorWindow>();
			window.titleContent = new GUIContent("Scene Workflow Editor");
		}

		private void OnEnable()
		{
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/SceneTransition/Editor/GraphViews/SceneWorkflowGraphViewCss.uss");

			if (styleSheet != null)
				rootVisualElement.styleSheets.Add(styleSheet);

			_graphView                = new SceneWorkflowGraphView();
			_graphView.style.flexGrow = 1;

			_graphView.AddToClassList("workflow-graph-view");

			rootVisualElement.Add(_graphView);
		}

		private void OnDisable()
		{
			rootVisualElement.Remove(_graphView);
		}
	}
}