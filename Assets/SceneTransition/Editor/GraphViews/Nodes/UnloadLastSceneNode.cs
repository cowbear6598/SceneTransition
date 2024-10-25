using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class UnloadLastSceneNode : WorkflowNode
	{
		public virtual  OperationType OperationType         => OperationType.UnloadLastScene;
		public override OperationData CreateOperationData() => new UnloadLastSceneOperationData(JsonUtility.ToJson(NodeData));

		public UnloadLastSceneNode() : base("移除上一個場景") { }
	}
}