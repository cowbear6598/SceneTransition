using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class RemoveEdgesCommand : IGraphViewCommand
	{
		private readonly List<Edge>               _edges;
		private readonly List<EdgeConnectionData> _connectionData;

		public RemoveEdgesCommand(List<Edge> edges)
		{
			_connectionData = new List<EdgeConnectionData>();

			_edges = edges;

			foreach (var edge in _edges)
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
			Debug.Log("RemoveEdgesCommand.Execute");

			foreach (var edge in _edges)
			{
				edge.input.Disconnect(edge);
				edge.output.Disconnect(edge);

				graphView.RemoveElement(edge);
			}
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			Debug.Log("RemoveEdgesCommand.Undo");

			for (var i = 0; i < _edges.Count; i++)
			{
				var nodes = graphView.Query<WorkflowNode>().ToList();

				var inputNodeId  = _connectionData[i].InputId;
				var outputNodeId = _connectionData[i].OutputId;

				var inputNode  = nodes.FirstOrDefault(node => node.Id == inputNodeId);
				var outputNode = nodes.FirstOrDefault(node => node.Id == outputNodeId);

				if (inputNode == null || outputNode == null)
					continue;

				_edges[i] = inputNode.Input.ConnectTo(outputNode.Output);

				graphView.AddElement(_edges[i]);
			}
		}
	}
}