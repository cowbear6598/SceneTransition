using SceneTransition.Editor.GraphViews.Nodes;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.History.Command
{
	internal class AddNodeCommand : IGraphViewCommand
	{
		private readonly WorkflowNode _workflowNode;
		private readonly Vector2      _position;

		public AddNodeCommand(WorkflowNode workflowNode, Vector2 position)
		{
			_workflowNode = workflowNode;
			_position     = position;
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			_workflowNode.SetPosition(new Rect(_position, Vector2.zero));
			graphView.AddElement(_workflowNode);
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			graphView.RemoveElement(_workflowNode);
		}
	}
}