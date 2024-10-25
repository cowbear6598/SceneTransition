using SceneTransition.Editor.GraphViews.Nodes;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Command
{
	public class MoveNodeCommand : IGraphViewCommand
	{
		private readonly WorkflowNode _node;
		private readonly Vector2      _oldPosition;
		private readonly Vector2      _newPosition;

		public MoveNodeCommand(WorkflowNode node, Vector2 oldPosition, Vector2 newPosition)
		{
			_node        = node;
			_oldPosition = oldPosition;
			_newPosition = newPosition;
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			_node.SetPosition(new Rect(_newPosition, Vector2.zero));
		}
		public void Undo(SceneWorkflowGraphView graphView)
		{
			_node.SetPosition(new Rect(_oldPosition, Vector2.zero));
		}
	}
}