using System.Collections.Generic;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Command
{
	internal class MoveNodeCommand : IGraphViewCommand
	{
		private readonly List<WorkflowNode> _node;
		private readonly List<Vector2>      _oldPosition;
		private readonly List<Vector2>      _newPosition;

		public MoveNodeCommand(List<WorkflowNode> node, List<Vector2> oldPosition, List<Vector2> newPosition)
		{
			_node        = node;
			_oldPosition = oldPosition;
			_newPosition = newPosition;
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			for (var i = 0; i < _node.Count; i++)
			{
				_node[i].SetPosition(new Rect(_newPosition[i], Vector2.zero));
				_node[i].UpdatePosition(_newPosition[i]);
			}
		}
		public void Undo(SceneWorkflowGraphView graphView)
		{
			for (var i = 0; i < _node.Count; i++)
			{
				_node[i].SetPosition(new Rect(_oldPosition[i], Vector2.zero));
				_node[i].UpdatePosition(_oldPosition[i]);
			}
		}
	}
}