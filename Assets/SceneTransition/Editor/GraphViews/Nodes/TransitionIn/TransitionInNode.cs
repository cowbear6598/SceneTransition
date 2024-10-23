using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.TransitionIn
{
	public class TransitionInNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.TransitionIn;

		public TransitionInNode() : base("轉場進入") { }
	}
}