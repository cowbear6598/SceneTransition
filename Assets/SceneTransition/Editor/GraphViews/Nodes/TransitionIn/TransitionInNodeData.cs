using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.TransitionIn
{
	public class TransitionInNodeData : NodeData
	{
		public override OperationType Type         => OperationType.TransitionIn;
		public override Settings      ToSettings() => new TransitionInSettings();
	}
}