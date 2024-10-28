using System.Collections.Generic;
using SceneTransition.Editor.GraphViews.Nodes;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class RemoveNodesCommand : IGraphViewCommand
	{
		private readonly List<WorkflowNode> _nodes;

		public RemoveNodesCommand(List<WorkflowNode> nodes)
		{
			_nodes = nodes;
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			foreach (var node in _nodes)
			{
				graphView.RemoveElement(node);
			}
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			foreach (var node in _nodes)
			{
				graphView.AddElement(node);
			}
		}
	}
}