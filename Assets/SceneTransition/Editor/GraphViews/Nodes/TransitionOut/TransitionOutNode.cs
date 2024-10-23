using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.TransitionOut
{
	public class TransitionOutNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.TransitionOut;

		public TransitionOutNode() : base("轉場退出") { }
	}
}