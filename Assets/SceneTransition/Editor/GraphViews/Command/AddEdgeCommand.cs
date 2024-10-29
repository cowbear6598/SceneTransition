using System.Linq;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class AddEdgeCommand : IGraphViewCommand
	{
		private bool _isInitialized = true;

		private readonly EdgeConnectionData _connectionData;

		public AddEdgeCommand(Edge edge)
		{
			var inputNodeId  = (edge.input.node as WorkflowNode).Id;
			var outputNodeId = (edge.output.node as WorkflowNode).Id;

			_connectionData = new EdgeConnectionData
			{
				InputId  = inputNodeId,
				OutputId = outputNodeId,
			};
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			Debug.Log("AddEdgeCommand.Execute");

			if (_isInitialized)
			{
				_isInitialized = false;
				return;
			}

			var workflowNodes = graphView.Query<WorkflowNode>().ToList();

			var inputNode  = workflowNodes.FirstOrDefault(node => node.Id == _connectionData.InputId);
			var outputNode = workflowNodes.FirstOrDefault(node => node.Id == _connectionData.OutputId);

			var edges = graphView.Query<Edge>().ToList();

			var edge = edges.FirstOrDefault(e =>
			{
				var inputId  = (e.input.node as WorkflowNode).Id;
				var outputId = (e.output.node as WorkflowNode).Id;

				return inputId == _connectionData.InputId && outputId == _connectionData.OutputId;
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

		public void Undo(SceneWorkflowGraphView graphView)
		{
			Debug.Log("AddEdgeCommand.Undo");

			var edges = graphView.Query<Edge>().ToList();

			var edge = edges.FirstOrDefault(e =>
			{
				var inputId  = (e.input.node as WorkflowNode).Id;
				var outputId = (e.output.node as WorkflowNode).Id;

				Debug.Log(inputId + " " + outputId + " " + _connectionData.InputId + " " + _connectionData.OutputId);

				return inputId == _connectionData.InputId && outputId == _connectionData.OutputId;
			});

			if (edge != null)
			{
				edge.input.DisconnectAll();
				edge.output.DisconnectAll();
				graphView.RemoveElement(edge);
			}
		}
	}
}