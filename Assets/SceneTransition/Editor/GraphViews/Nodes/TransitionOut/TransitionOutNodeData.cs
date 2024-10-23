using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.GraphViews.Nodes.TransitionOut
{
	public class TransitionOutNodeData : NodeData
	{
		public override OperationType Type         => OperationType.TransitionOut;
		public override Settings      ToSettings() => new TransitionOutSettings();
	}
}