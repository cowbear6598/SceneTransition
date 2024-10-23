using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.UnloadAllScenes
{
	public class UnloadAllScenesNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.UnloadAllScenes;

		public UnloadAllScenesNode() : base("移除所有場景") { }
	}
}