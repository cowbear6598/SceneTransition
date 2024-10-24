using SceneTransition.Operations;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class UnloadLastSceneNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.UnloadLastScene;

		public UnloadLastSceneNode() : base("移除上一個場景") { }
	}
}