using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionOutNode : WorkflowNode
	{
		public virtual OperationType OperationType => OperationType.TransitionOut;
		public override OperationData CreateOperationData() => new TransitionOutOperationData(
			JsonUtility.ToJson(NodeData)
		);

		public TransitionOutNode() : base("轉場退出") { }
	}
}