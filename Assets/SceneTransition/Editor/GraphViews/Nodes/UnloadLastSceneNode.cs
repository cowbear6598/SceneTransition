using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class UnloadLastSceneNode : WorkflowNode
	{
		public UnloadLastSceneNode(SceneWorkflowGraphView graphView) : base("移除上一個場景", graphView) { }

		protected override OperationData ToOperationData(string nodeData) => new UnloadLastSceneOperationData(nodeData);
	}
}