using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionInNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.TransitionIn;

		public TransitionInNode() : base("轉場進入") { }

		protected override OperationData MakeOperationData(string nodeData) => new TransitionInOperationData(nodeData);
	}
}