using SceneTransition.Operations;
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

		public override OperationType OperationType => OperationType.LoadScene;

		private readonly ObjectField _objectField;

		public LoadSceneNode() : base("讀取場景")
		{
			_objectField = new ObjectField("場景資源")
			{
				objectType        = typeof(Object),
				allowSceneObjects = false,
				style             = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_objectField.RegisterValueChangedCallback(evt =>
			{
				if (evt.newValue == null)
				{
					SceneAsset = null;

					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(evt.newValue);

				if (!assetPath.EndsWith(".unity"))
				{
					Debug.LogError($"{evt.newValue.name} 不是場景。");

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
					Debug.LogError($"場景資源 '{evt.newValue.name}' 不在 Addressable 資源中");
					_objectField.SetValueWithoutNotify(null);
					SceneAsset = null;
				}
			});

			mainContainer.Add(_objectField);
		}

		public void SetSceneAsset(AssetReference sceneAsset)
		{
			SceneAsset = sceneAsset;

			_objectField.SetValueWithoutNotify(sceneAsset?.editorAsset);
		}
	}
}