using SceneTransition.ScriptableObjects.Data;
using UnityEditor;
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

		public LoadSceneNode() : base("讀取場景")
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
					_sceneAsset = null;

					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(e.newValue);

				if (!assetPath.EndsWith(".unity"))
				{
					EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不是場景資源", "確定");

					_objectField.SetValueWithoutNotify(null);
					_sceneAsset = null;

					return;
				}

				var guid = AssetDatabase.AssetPathToGUID(assetPath);

				if (UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid) != null)
				{
					_sceneAsset = new AssetReference(guid);
				}
				else
				{
					EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不在 Addressable 中", "確定");

					_objectField.SetValueWithoutNotify(null);
					_sceneAsset = null;
				}
			});

			mainContainer.Add(_objectField);
		}

		protected override OperationData ToOperationData(string nodeData)
			=> new LoadSceneOperationData(nodeData, _sceneAsset);

		public override void LoadFromData(OperationData operationData)
		{
			var data = operationData as LoadSceneOperationData;

			_sceneAsset = data.SceneAsset;

			_objectField.SetValueWithoutNotify(_sceneAsset?.editorAsset);
		}

		public override bool IsValidateToSave()
		{
			if (_sceneAsset != null)
				return true;

			EditorUtility.DisplayDialog("錯誤", "場景資源未設定", "確定");

			return false;
		}
	}
}