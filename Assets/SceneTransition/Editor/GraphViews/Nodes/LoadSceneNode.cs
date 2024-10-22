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
				style             = { marginTop = 8, marginBottom = 8 }
			};

			objectField.RegisterValueChangedCallback(evt =>
			{
				SceneReference = evt.newValue != null
					? new AssetReference(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(evt.newValue)))
					: null;
			});

			mainContainer.Add(objectField);
		}
	}
}