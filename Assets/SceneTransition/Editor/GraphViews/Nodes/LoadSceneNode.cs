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
		public AssetReference SceneAsset;

		public override OperationType OperationType => OperationType.LoadScene;

		public LoadSceneNode() : base("讀取場景")
		{
			var objectField = new ObjectField("場景資源")
			{
				objectType        = typeof(Object),
				allowSceneObjects = false,
				style             = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			objectField.RegisterValueChangedCallback(evt =>
			{
				if (evt.newValue == null)
				{
					SceneAsset = null;

					return;
				}

				var assetPath = AssetDatabase.GetAssetPath((Object)evt.newValue);

				if (!assetPath.EndsWith(".unity"))
				{
					Debug.LogError($"{evt.newValue.name} 不是場景。");

					objectField.SetValueWithoutNotify(null);
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
					objectField.SetValueWithoutNotify(null);
					SceneAsset = null;
				}
			});


			mainContainer.Add(objectField);
		}
	}
}