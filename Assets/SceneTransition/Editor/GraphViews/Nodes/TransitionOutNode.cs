using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class TransitionOutNode : WorkflowNode
	{
		public TransitionOutNode(SceneWorkflowGraphView graphView) : base("轉場退出", graphView) { }

		protected override OperationData ToOperationData(string nodeData) => new TransitionOutOperationData(nodeData);
	}
}