using SceneTransition.Editor.GraphViews.Command.Common;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor.Experimental.GraphView;

namespace SceneTransition.Editor.GraphViews.Command
{
	internal class AddEdgeCommand : IGraphViewCommand
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
			if (_isInitialized)
			{
				_isInitialized = false;
				return;
			}

			EdgeUtils.RemoveOldEdge(graphView, _connectionData);
			EdgeUtils.AddEdge(graphView, _connectionData);
		}

		public void Undo(SceneWorkflowGraphView graphView)
			=> EdgeUtils.RemoveOldEdge(graphView, _connectionData);
	}
}