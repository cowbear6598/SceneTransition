using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionInNode : WorkflowNode
	{
		public virtual OperationType OperationType => OperationType.TransitionIn;
		public override OperationData CreateOperationData() => new TransitionInOperationData(
			JsonUtility.ToJson(NodeData)
		);

		public TransitionInNode() : base("轉場進入") { }
	}
}