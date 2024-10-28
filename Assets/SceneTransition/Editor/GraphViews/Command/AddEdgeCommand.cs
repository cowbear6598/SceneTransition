using System.Linq;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class AddEdgeCommand : IGraphViewCommand
	{
		private Edge _edge;

		private readonly EdgeConnectionData _connectionData;

		public AddEdgeCommand(Edge edge)
		{
			_edge = edge;

			var inputNodeId  = (_edge.input.node as WorkflowNode).Id;
			var outputNodeId = (_edge.output.node as WorkflowNode).Id;

			_connectionData = new EdgeConnectionData
			{
				InputId  = inputNodeId,
				OutputId = outputNodeId,
			};
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			Debug.Log("AddEdgeCommand.Execute");

			if (_edge != null)
			{
				Debug.LogWarning("Edge already exists");
				return;
			}

			var workflowNodes = graphView.Query<WorkflowNode>().ToList();

			var inputNode  = workflowNodes.FirstOrDefault(node => node.Id == _connectionData.InputId);
			var outputNode = workflowNodes.FirstOrDefault(node => node.Id == _connectionData.OutputId);

			if (inputNode == null || outputNode == null)
				return;

			_edge = inputNode.Input.ConnectTo(outputNode.Output);

			graphView.AddElement(_edge);
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			Debug.Log("AddEdgeCommand.Undo");

			_edge.input?.DisconnectAll();
			_edge.output?.DisconnectAll();

			graphView.RemoveElement(_edge);

			_edge = null;
		}
	}
}