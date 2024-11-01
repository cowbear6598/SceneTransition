using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class UnloadLastSceneNode : WorkflowNode
	{
		public UnloadLastSceneNode() : base("移除上一個場景") { }

		protected override OperationData ToOperationData(string nodeData) => new UnloadLastSceneOperationData(nodeData);
	}
}