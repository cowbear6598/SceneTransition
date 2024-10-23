using SceneTransition.Editor.GraphViews;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.Windows
{
	public class SceneWorkflowEditorWindow : EditorWindow
	{
		private SceneWorkflowGraphView _graphView;

		[MenuItem("SceneTransition/流程編輯器")]
		public static void OpenWindow()
		{
			var window = GetWindow<SceneWorkflowEditorWindow>();
			window.titleContent = new GUIContent("場景轉換流程編輯器");
		}

		private void OnEnable()
		{
			CreateToolbar();

			_graphView                = new SceneWorkflowGraphView();
			_graphView.style.flexGrow = 1;

			rootVisualElement.Add(_graphView);
		}

		private void OnDisable()
		{
			rootVisualElement.Remove(_graphView);
		}

		private void CreateToolbar()
		{
			var toolbar = new Toolbar();

			var loadButton    = new ToolbarButton(Button_Load) { text    = "載入" };
			var saveButton    = new ToolbarButton(Button_Save) { text    = "儲存" };
			var saveAsButton  = new ToolbarButton(Button_SaveAs) { text  = "另存新檔" };
			var showAllButton = new ToolbarButton(Button_ShowAll) { text = "顯示全部" };

			toolbar.Add(loadButton);
			toolbar.Add(saveButton);
			toolbar.Add(saveAsButton);
			toolbar.Add(new ToolbarSpacer());
			toolbar.Add(showAllButton);

			rootVisualElement.Add(toolbar);
		}

		private void Button_Load() { }

		private void Button_Save() { }

		private void Button_SaveAs() { }

		private void Button_ShowAll() => _graphView.FrameAll();
	}
}