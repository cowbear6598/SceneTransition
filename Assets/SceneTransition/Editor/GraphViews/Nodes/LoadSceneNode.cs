using SceneTransition.ScriptableObjects.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class LoadSceneNode : WorkflowNode
	{
		public AssetReference SceneAsset { get; private set; }

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
					SceneAsset = null;

					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(e.newValue);

				if (!assetPath.EndsWith(".unity"))
				{
					EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不是場景資源", "確定");

					_objectField.SetValueWithoutNotify(null);
					SceneAsset = null;

					return;
				}

				var guid = AssetDatabase.AssetPathToGUID(assetPath);

				if (UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid) != null)
				{
					SceneAsset = new AssetReference(guid);
				}
				else
				{
					EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不在 Addressable 中", "確定");

					_objectField.SetValueWithoutNotify(null);
					SceneAsset = null;
				}
			});

			mainContainer.Add(_objectField);
		}

		protected override OperationData MakeOperationData(string nodeData)
			=> new LoadSceneOperationData(nodeData, SceneAsset);

		public override void LoadFromData(OperationData operationData)
		{
			var data = operationData as LoadSceneOperationData;

			SceneAsset = data.SceneAsset;

			_objectField.SetValueWithoutNotify(SceneAsset?.editorAsset);
		}

		public override bool IsValidateToSave()
		{
			if (SceneAsset == null)
				throw new System.Exception("請選擇場景資源");

			return true;
		}
	}
}