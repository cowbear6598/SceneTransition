using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class UnloadAllScenesNode : WorkflowNode
	{
		public UnloadAllScenesNode(SceneWorkflowGraphView graphView) : base("移除所有場景", graphView) { }

		protected override OperationData ToOperationData(string nodeData) => new UnloadAllScenesOperationData(nodeData);
	}
}