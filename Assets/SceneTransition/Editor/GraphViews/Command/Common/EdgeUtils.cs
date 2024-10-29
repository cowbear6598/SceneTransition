using System.Linq;
using JetBrains.Annotations;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Command.Common
{
	internal static class EdgeUtils
	{
		public static void AddEdge(GraphView graphView, EdgeConnectionData connectionData)
		{
			var workflowNodes = graphView.Query<WorkflowNode>().ToList();

			var inputNode  = workflowNodes.First(node => node.Id == connectionData.InputId);
			var outputNode = workflowNodes.First(node => node.Id == connectionData.OutputId);

			var edge = inputNode.Input.ConnectTo(outputNode.Output);

			graphView.AddElement(edge);
		}

		private static Edge GetEdge(GraphView graphView, EdgeConnectionData connectionData)
		{
			var edges = graphView.Query<Edge>().ToList();

			var edge = edges.FirstOrDefault(e =>
			{
				var inputId  = (e.input.node as WorkflowNode).Id;
				var outputId = (e.output.node as WorkflowNode).Id;

				return inputId == connectionData.InputId && outputId == connectionData.OutputId;
			});

			return edge;
		}

		public static void RemoveOldEdge(GraphView graphView, EdgeConnectionData connectionData)
		{
			var edge = GetEdge(graphView, connectionData);

			if (edge == null)
				return;

			edge.input.DisconnectAll();
			edge.output.DisconnectAll();

			graphView.RemoveElement(edge);
		}
	}
}