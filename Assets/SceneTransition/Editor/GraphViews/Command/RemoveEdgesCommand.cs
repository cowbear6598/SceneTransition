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
			Debug.Log("RemoveEdgesCommand.Execute");

			foreach (var data in _connectionData)
			{
				var edges = graphView.Query<Edge>().ToList();

				var edge = edges.FirstOrDefault(e =>
				{
					var inputId  = (e.input.node as WorkflowNode).Id;
					var outputId = (e.output.node as WorkflowNode).Id;

					return inputId == data.InputId && outputId == data.OutputId;
				});

				if (edge != null)
				{
					edge.input.DisconnectAll();
					edge.output.DisconnectAll();
					graphView.RemoveElement(edge);
				}
			}
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			Debug.Log("RemoveEdgesCommand.Undo");

			foreach (var data in _connectionData)
			{
				var workflowNodes = graphView.Query<WorkflowNode>().ToList();

				var inputNode  = workflowNodes.FirstOrDefault(node => node.Id == data.InputId);
				var outputNode = workflowNodes.FirstOrDefault(node => node.Id == data.OutputId);

				var edges = graphView.Query<Edge>().ToList();

				var edge = edges.FirstOrDefault(e =>
				{
					var inputId  = (e.input.node as WorkflowNode).Id;
					var outputId = (e.output.node as WorkflowNode).Id;

					return inputId == data.InputId && outputId == data.OutputId;
				});

				if (edge != null)
				{
					edge.input.DisconnectAll();
					edge.output.DisconnectAll();
					graphView.RemoveElement(edge);
				}

				if (inputNode == null || outputNode == null)
					return;

				var newEdge = inputNode.Input.ConnectTo(outputNode.Output);

				graphView.AddElement(newEdge);
			}
		}
	}
}