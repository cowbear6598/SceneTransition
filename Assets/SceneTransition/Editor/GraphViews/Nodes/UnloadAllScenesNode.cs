using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class UnloadAllScenesNode : WorkflowNode
	{
		public override OperationType OperationType => OperationType.UnloadAllScenes;

		public UnloadAllScenesNode() : base("移除所有場景") { }

		protected override OperationData MakeOperationData(string nodeData) => new UnloadAllScenesOperationData(nodeData);
	}
}