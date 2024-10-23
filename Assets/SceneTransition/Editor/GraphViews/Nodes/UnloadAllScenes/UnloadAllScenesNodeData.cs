using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.UnloadAllScenes
{
	public class UnloadAllScenesNodeData : NodeData
	{
		public override OperationType Type         => OperationType.UnloadAllScenes;
		public override Settings      ToSettings() => new UnloadAllScenesSettings();
	}
}