using System;
using SceneTransition.Editor.GraphViews;
using SceneTransition.ScriptableObjects;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.Windows
{
	public class SceneWorkflowEditorWindow : EditorWindow
	{
		private SceneWorkflowGraphView _graphView;
		private SceneWorkflowAsset     _workflowAsset;

		[MenuItem("SceneTransition/流程編輯器")]
		public static void OpenWindow()
		{
			var window = GetWindow<SceneWorkflowEditorWindow>();
			window.titleContent = new GUIContent("未命名");
		}

		private void OnEnable()
		{
			CreateToolbar();

			_graphView                = new SceneWorkflowGraphView(this);
			_graphView.style.flexGrow = 1;

			rootVisualElement.Add(_graphView);

			rootVisualElement.RegisterCallback<KeyDownEvent>(OnKeyDown);
		}

		private void OnDisable()
		{
			rootVisualElement.Remove(_graphView);
		}

		#region Toolbar

		private void CreateToolbar()
		{
			var toolbar = new Toolbar();

			var loadButton    = new ToolbarButton(() => Load()) { text = "載入" };
			var saveButton    = new ToolbarButton(Save) { text         = "儲存" };
			var saveAsButton  = new ToolbarButton(SaveAs) { text       = "另存新檔" };
			var showAllButton = new ToolbarButton(ShowAll) { text      = "顯示全部" };

			toolbar.Add(loadButton);
			toolbar.Add(saveButton);
			toolbar.Add(saveAsButton);
			toolbar.Add(new ToolbarSpacer());
			toolbar.Add(showAllButton);

			rootVisualElement.Add(toolbar);
		}

		#endregion

		public void Load(SceneWorkflowAsset workflowAsset = null)
		{
			SceneWorkflowAsset asset;

			if (workflowAsset == null)
			{
				var path = EditorUtility.OpenFilePanelWithFilters(
					"載入場景轉換流程",
					"Assets",
					new[] { "場景轉換流程", "asset" }
				);

				if (string.IsNullOrEmpty(path))
					return;

				path = FileUtil.GetProjectRelativePath(path);

				asset = AssetDatabase.LoadAssetAtPath<SceneWorkflowAsset>(path);
			}
			else
				asset = workflowAsset;

			_workflowAsset = asset;
			_graphView.LoadFromAsset(asset);
			titleContent = new GUIContent($"{asset.name}");
		}

		private void Save()
		{
			if (_workflowAsset == null)
			{
				SaveAs();

				return;
			}

			_workflowAsset.ClearOperations();
			_graphView.SaveToAsset(_workflowAsset);
		}

		private void SaveAs()
		{
			var path = EditorUtility.SaveFilePanelInProject(
				"儲存場景轉換流程",
				"場景轉換流程",
				"asset",
				"請選擇儲存位置"
			);

			if (string.IsNullOrEmpty(path))
				return;

			try
			{
				_workflowAsset = CreateInstance<SceneWorkflowAsset>();

				_graphView.SaveToAsset(_workflowAsset);

				AssetDatabase.CreateAsset(_workflowAsset, path);
				AssetDatabase.SaveAssets();

				titleContent = new GUIContent($"{path.Substring(path.LastIndexOf('/') + 1)}");
			}
			catch (Exception e)
			{
				EditorUtility.DisplayDialog("儲存失敗", e.Message, "確定");
			}
		}

		private void ShowAll() => _graphView.FrameAll();

		private void OnKeyDown(KeyDownEvent e)
		{
			if (!e.actionKey)
				return;

			switch (e.keyCode)
			{
				case KeyCode.S:
					if (e.shiftKey) // 另存新檔
						SaveAs();
					else // 儲存
						Save();

					e.StopPropagation();

					break;
			}
		}
	}
}