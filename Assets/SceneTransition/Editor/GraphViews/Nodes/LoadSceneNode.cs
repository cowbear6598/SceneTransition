using System.IO;
using System.Linq;
using SceneTransition.Editor.GraphViews.History.Command;
using SceneTransition.ScriptableObjects.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class LoadSceneNode : WorkflowNode
	{
		private string _sceneName;
		private float  _delayTime;

		private readonly PopupField<string> _scenePopup;
		private readonly FloatField         _delayTimeField;

		public LoadSceneNode(SceneWorkflowGraphView graphView) : base("讀取場景", graphView)
		{
			var sceneNames = EditorBuildSettings.scenes
			                                    .Select(s => Path.GetFileNameWithoutExtension(s.path))
			                                    .ToList();

			if (sceneNames.Count == 0)
			{
				Debug.LogError("沒有場景可以選擇讀取");
				return;
			}

			_scenePopup = new PopupField<string>("場景名稱", sceneNames, 0)
			{
				style = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 }
			};

			_scenePopup.RegisterValueChangedCallback(e =>
			{
				ChangeSceneName(e.newValue);
			});

			_delayTimeField = new FloatField("等待時間")
			{
				style = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_delayTimeField.RegisterValueChangedCallback(e =>
			{
				ChangeDelayTime(e.newValue);
			});

			mainContainer.Add(_scenePopup);
			mainContainer.Add(_delayTimeField);
		}

		private void ChangeDelayTime(float delayTime)
		{
			var oldData = CreateOperationData();

			_delayTime = Mathf.Clamp(delayTime, 0, float.MaxValue);

			var newData = CreateOperationData();

			var command = new ChangePropertyCommand(this, newData, oldData);
			_graphView.ExecuteCommand(command);
		}

		private void ChangeSceneName(string sceneName)
		{
			var oldData = CreateOperationData();

			_sceneName = sceneName;

			var newData = CreateOperationData();
			var command = new ChangePropertyCommand(this, newData, oldData);

			_graphView.ExecuteCommand(command);
		}

		protected override OperationData ToOperationData(string nodeData)
			=> new LoadSceneOperationData(nodeData, _sceneName, _delayTime);

		internal override void LoadFromData(OperationData operationData)
		{
			var data = operationData as LoadSceneOperationData;

			_sceneName = data.SceneName;
			_delayTime = data.DelayTime;

			if (!_scenePopup.choices.Contains(_sceneName))
			{
				Debug.LogError($"場景名稱 {_sceneName} 不存在");
				return;
			}

			_scenePopup.SetValueWithoutNotify(_sceneName);
			_delayTimeField.SetValueWithoutNotify(_delayTime);
		}

		internal override bool IsValidateToSave()
		{
			var sceneExists = EditorBuildSettings.scenes
			                                     .Any(s => Path.GetFileNameWithoutExtension(s.path) == _sceneName);

			if (sceneExists)
				return true;

			EditorUtility.DisplayDialog("錯誤", "選擇的場景不在 Build Settings 中", "確定");
			return false;
		}
	}
}