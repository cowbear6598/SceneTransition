using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.UnloadLastScene
{
	public class UnloadLastSceneNodeData : NodeData
	{
		public override OperationType Type         => OperationType.UnloadLastScene;
		public override Settings      ToSettings() => new UnloadLastSceneSettings();
	}
}