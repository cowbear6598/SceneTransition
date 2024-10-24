using SceneTransition.Operations;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionOutNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.TransitionOut;

		public TransitionOutNode() : base("轉場退出") { }
	}
}