using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class UnloadAllScenesNode : WorkflowNode
	{
		public UnloadAllScenesNode() : base("移除所有場景") { }

		protected override OperationData ToOperationData(string nodeData) => new UnloadAllScenesOperationData(nodeData);
	}
}