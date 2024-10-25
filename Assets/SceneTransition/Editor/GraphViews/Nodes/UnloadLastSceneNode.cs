using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class UnloadLastSceneNode : WorkflowNode
	{
		public UnloadLastSceneNode() : base("移除上一個場景") { }

		protected override OperationData MakeOperationData(string nodeData) => new UnloadLastSceneOperationData(nodeData);
	}
}