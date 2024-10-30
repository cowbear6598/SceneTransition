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
		[SerializeField] private string _assetPath;

		private SceneWorkflowGraphView _graphView;
		private SceneWorkflowAsset     _workflowAsset;

		private string _title;

		[MenuItem("SceneTransition/流程編輯器")]
		public static void OpenWindow()
		{
			var window = GetWindow<SceneWorkflowEditorWindow>();
			window.titleContent = new GUIContent("未命名");
		}

		private void OnEnable()
		{
			CreateToolbar();

			_title = "未命名";

			_graphView                = new SceneWorkflowGraphView();
			_graphView.style.flexGrow = 1;

			rootVisualElement.Add(_graphView);

			rootVisualElement.RegisterCallback<KeyDownEvent>(KeyMap);

			_graphView.RegisterOnDirtyChanged(OnGraphViewDirtyChanged);

			// 防止 Unity Compile 時丟失資料
			if (string.IsNullOrEmpty(_assetPath))
				return;

			_workflowAsset = AssetDatabase.LoadAssetAtPath<SceneWorkflowAsset>(_assetPath);

			if (_workflowAsset == null)
				return;

			Load(_workflowAsset);
		}

		private void OnDisable()
		{
			rootVisualElement.Remove(_graphView);

			// 防止 Unity Compile 時丟失資料
			if (_workflowAsset != null)
				_assetPath = AssetDatabase.GetAssetPath(_workflowAsset);
		}

		#region 工具列

		private void CreateToolbar()
		{
			var toolbar = new Toolbar();

			var loadButton    = new ToolbarButton(() => Load()) { text                = "載入" };
			var saveButton    = new ToolbarButton(Save) { text                        = "儲存" };
			var saveAsButton  = new ToolbarButton(SaveAs) { text                      = "另存新檔" };
			var showAllButton = new ToolbarButton(() => _graphView.FrameAll()) { text = "顯示全部" };

			toolbar.Add(loadButton);
			toolbar.Add(saveButton);
			toolbar.Add(saveAsButton);
			toolbar.Add(new ToolbarSpacer());
			toolbar.Add(showAllButton);

			rootVisualElement.Add(toolbar);
		}

		#endregion

		#region 儲存/載入

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
			{
				asset = workflowAsset;
			}

			_workflowAsset = asset;
			_graphView.LoadFromAsset(asset);

			_title       = asset.name;
			titleContent = new GUIContent($"{_title}");
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

			EditorUtility.SetDirty(_workflowAsset);
			AssetDatabase.SaveAssets();
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

				_title       = _workflowAsset.name;
				titleContent = new GUIContent($"{_workflowAsset.name}");
			}
			catch (Exception e)
			{
				EditorUtility.DisplayDialog("儲存失敗", e.Message, "確定");
			}
		}

		#endregion

		#region 快捷鍵

		private void KeyMap(KeyDownEvent e)
		{
			if (!e.actionKey)
				return;

			switch (e.keyCode)
			{
				case KeyCode.Z:
					RedoUndoKeyMap(e);
					break;

				case KeyCode.S:
					SaveKeyMap(e);
					break;
			}
		}
		private void SaveKeyMap(KeyDownEvent e)
		{
			e.StopPropagation();

			if (e.shiftKey) // 另存新檔
			{
				SaveAs();
			}
			else // 儲存
			{
				Save();
			}
		}

		private void RedoUndoKeyMap(KeyDownEvent e)
		{
			e.StopPropagation();

			if (e.shiftKey)
			{
				_graphView.Redo();
			}
			else
			{
				_graphView.Undo();
			}
		}

		#endregion

		private void OnGraphViewDirtyChanged(bool isDirty)
		{
			titleContent = new GUIContent($"{_title}{(isDirty ? "*" : "")}");
		}
	}
}