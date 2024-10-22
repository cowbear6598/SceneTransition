using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class LoadSceneNode : Node
	{
		public string         NodeID         { get; private set; }
		public Port           Input          { get; private set; }
		public Port           Output         { get; private set; }
		public AssetReference SceneReference { get; private set; }

		public LoadSceneNode()
		{
			NodeID = System.Guid.NewGuid().ToString();

			title = "讀取場景";

			Input          = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
			Input.portName = "執行";
			inputContainer.Add(Input);

			Output          = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
			Output.portName = "下一步";
			outputContainer.Add(Output);

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

			AddToClassList("scene-workflow-node");

			RefreshExpandedState();
			RefreshPorts();
		}
	}
}