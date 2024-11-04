using SceneTransition.Editor.GraphViews.History.Command;
using SceneTransition.ScriptableObjects.Data;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class LoadSceneNode : WorkflowNode
	{
		private AssetReference _sceneAsset;
		private float          _delayTime;

		private readonly ObjectField _objectField;
		private readonly FloatField  _delayTimeField;

		public LoadSceneNode(SceneWorkflowGraphView graphView) : base("讀取場景", graphView)
		{
			_objectField = new ObjectField("場景資源")
			{
				objectType        = typeof(Object),
				allowSceneObjects = false,
				style             = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_objectField.RegisterValueChangedCallback(e =>
			{
				if (e.newValue == null)
				{
					ChangeSceneAsset(null);
					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(e.newValue);

				if (!assetPath.EndsWith(".unity"))
				{
					EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不是場景資源", "確定");
					_objectField.SetValueWithoutNotify(_sceneAsset?.editorAsset);
					return;
				}

				var guid             = AssetDatabase.AssetPathToGUID(assetPath);
				var addressableAsset = AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid);

				if (addressableAsset == null)
				{
					EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不在 Addressable 中", "確定");
					_objectField.SetValueWithoutNotify(_sceneAsset?.editorAsset);
					return;
				}

				ChangeSceneAsset(new AssetReference(guid));
			});

			_delayTimeField = new FloatField("等待時間")
			{
				style = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_delayTimeField.RegisterValueChangedCallback(e =>
			{
				ChangeDelayTime(e.newValue);
			});

			mainContainer.Add(_objectField);
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

		private void ChangeSceneAsset(AssetReference sceneAsset)
		{
			var oldData = CreateOperationData();

			_sceneAsset = sceneAsset;

			var newData = CreateOperationData();
			var command = new ChangePropertyCommand(this, newData, oldData);

			_graphView.ExecuteCommand(command);
		}

		protected override OperationData ToOperationData(string nodeData)
			=> new LoadSceneOperationData(nodeData, _sceneAsset, _delayTime);

		internal override void LoadFromData(OperationData operationData)
		{
			var data = operationData as LoadSceneOperationData;

			_sceneAsset = data.SceneAsset;
			_delayTime  = data.DelayTime;

			_objectField.SetValueWithoutNotify(_sceneAsset?.editorAsset);
			_delayTimeField.SetValueWithoutNotify(_delayTime);
		}

		internal override bool IsValidateToSave()
		{
			if (_sceneAsset != null)
				return true;

			EditorUtility.DisplayDialog("錯誤", "場景資源未設定", "確定");

			return false;
		}
	}
}