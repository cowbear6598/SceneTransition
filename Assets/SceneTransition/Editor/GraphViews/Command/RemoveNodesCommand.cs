using System.Collections.Generic;
using SceneTransition.Editor.GraphViews.Nodes;
using SceneTransition.ScriptableObjects.Data;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class RemoveNodesCommand : IGraphViewCommand
	{
		private readonly List<OperationData> _operationData = new();

		private List<WorkflowNode> _nodes;

		public RemoveNodesCommand(List<WorkflowNode> nodes) => _nodes = nodes;

		public void Execute(SceneWorkflowGraphView graphView)
		{
			foreach (var node in _nodes)
			{
				var operationData = node.CreateOperationData();

				_operationData.Add(operationData);

				graphView.RemoveElement(node);
			}
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			_nodes.Clear();

			_nodes = graphView.CreateNodes(_operationData);
		}
	}
}