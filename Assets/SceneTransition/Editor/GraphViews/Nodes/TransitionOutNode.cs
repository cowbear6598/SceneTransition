using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionOutNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.TransitionOut;

		public TransitionOutNode() : base("轉場退出") { }

		protected override OperationData MakeOperationData(string nodeData) => new TransitionOutOperationData(nodeData);
	}
}