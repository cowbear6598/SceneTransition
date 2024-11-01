using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class TransitionOutNode : WorkflowNode
	{
		public TransitionOutNode() : base("轉場退出") { }

		protected override OperationData ToOperationData(string nodeData) => new TransitionOutOperationData(nodeData);
	}
}