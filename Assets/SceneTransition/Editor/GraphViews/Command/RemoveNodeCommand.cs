using System.Collections.Generic;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class RemoveNodeCommand : IGraphViewCommand
	{
		private readonly WorkflowNode     _node;
		private readonly Vector2          _position;
		private readonly List<Connection> _connections;

		public RemoveNodeCommand(WorkflowNode node)
		{
			_node        = node;
			_position    = node.GetPosition().position;
			_connections = GetNodeConnections(node);
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			graphView.RemoveElement(_node);
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			_node.SetPosition(new Rect(_position, Vector2.zero));
			graphView.AddElement(_node);

			foreach (var connection in _connections)
			{
				if (connection.IsInput)
					_node.Input.ConnectTo(connection.Node.Output);
				else
					_node.Output.ConnectTo(connection.Node.Input);
			}
		}

		private List<Connection> GetNodeConnections(WorkflowNode node)
		{
			var connections = new List<Connection>();

			foreach (var edge in node.Input.connections)
			{
				if (edge.output.node is WorkflowNode connected)
					connections.Add(new Connection(connected, true));
			}

			foreach (var edge in node.Output.connections)
			{
				if (edge.input.node is WorkflowNode connected)
					connections.Add(new Connection(connected, false));
			}

			return connections;
		}

		private class Connection
		{
			public readonly WorkflowNode Node;
			public readonly bool         IsInput;

			public Connection(WorkflowNode node, bool isInput)
			{
				Node    = node;
				IsInput = isInput;
			}
		}
	}
}