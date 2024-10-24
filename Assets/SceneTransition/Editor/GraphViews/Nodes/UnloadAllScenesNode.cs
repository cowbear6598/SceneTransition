using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class UnloadAllScenesNode : WorkflowNode
	{
		public override OperationType OperationType         => OperationType.UnloadAllScenes;
		public override OperationData CreateOperationData() => new UnloadAllScenesOperationData(JsonUtility.ToJson(NodeData));

		public UnloadAllScenesNode() : base("移除所有場景") { }
	}
}