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

		private readonly ObjectField _objectField;

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

			mainContainer.Add(_objectField);
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
			=> new LoadSceneOperationData(nodeData, _sceneAsset);

		internal override void LoadFromData(OperationData operationData)
		{
			var data = operationData as LoadSceneOperationData;

			_sceneAsset = data.SceneAsset;

			_objectField.SetValueWithoutNotify(_sceneAsset?.editorAsset);
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