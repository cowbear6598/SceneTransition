using System.Collections.Generic;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class RemoveNodesCommand : IGraphViewCommand
	{
		private readonly List<NodeData>     _nodeData = new();
		private readonly List<WorkflowNode> _nodes;

		public RemoveNodesCommand(List<WorkflowNode> nodes) => _nodes = nodes;

		public void Execute(SceneWorkflowGraphView graphView)
		{
			foreach (var node in _nodes)
			{

			}
		}

		public void Undo(SceneWorkflowGraphView graphView) { }
	}
}