using SceneTransition.Operations;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class UnloadAllScenesNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.UnloadAllScenes;

		public UnloadAllScenesNode() : base("移除所有場景") { }
	}
}