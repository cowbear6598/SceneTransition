using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.UnloadLastScene
{
	public class UnloadLastSceneNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.UnloadLastScene;

		public UnloadLastSceneNode() : base("移除上一個場景") { }
	}
}