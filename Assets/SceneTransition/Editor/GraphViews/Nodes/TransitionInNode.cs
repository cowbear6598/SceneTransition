using SceneTransition.Operations;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionInNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.TransitionIn;

		public TransitionInNode() : base("轉場進入") { }
	}
}