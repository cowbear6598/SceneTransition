using System.Collections.Generic;
using SceneTransition.Editor.GraphViews.History.Command.Common;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor.Experimental.GraphView;

namespace SceneTransition.Editor.GraphViews.History.Command
{
	internal class RemoveEdgesCommand : IGraphViewCommand
	{
		private readonly List<EdgeConnectionData> _connectionData;

		public RemoveEdgesCommand(List<Edge> edges)
		{
			_connectionData = new List<EdgeConnectionData>();

			foreach (var edge in edges)
			{
				var inputNodeId  = (edge.input.node as WorkflowNode)?.Id;
				var outputNodeId = (edge.output.node as WorkflowNode)?.Id;

				_connectionData.Add(new EdgeConnectionData
				{
					InputId  = inputNodeId,
					OutputId = outputNodeId,
				});
			}
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			foreach (var data in _connectionData)
			{
				EdgeUtils.RemoveOldEdge(graphView, data);
			}
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			foreach (var data in _connectionData)
			{
				EdgeUtils.RemoveOldEdge(graphView, data);
				EdgeUtils.AddEdge(graphView, data);
			}
		}
	}
}