using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class LoadSceneNode : WorkflowNode
	{
		public AssetReference SceneReference { get; private set; }

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
					SceneReference = null;

					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(evt.newValue);

				if (!assetPath.EndsWith(".unity"))
				{
					Debug.LogError($"{evt.newValue.name} 不是場景。");

					objectField.SetValueWithoutNotify(null);
					SceneReference = null;

					return;
				}

				var guid = AssetDatabase.AssetPathToGUID(assetPath);

				if (UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid) != null)
				{
					SceneReference = new AssetReference(guid);
				}
				else
				{
					Debug.LogError($"場景資源 '{evt.newValue.name}' 不在 Addressable 資源中");
					objectField.SetValueWithoutNotify(null);
					SceneReference = null;
				}
			});


			mainContainer.Add(objectField);
		}
	}
}