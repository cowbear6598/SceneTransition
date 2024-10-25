using System.Collections.Generic;
using JetBrains.Annotations;
using SceneTransition.Editor.GraphViews.Command;

namespace SceneTransition.Editor.GraphViews
{
	public class SceneWorkflowGraphViewHistory
	{
		private readonly Stack<IGraphViewCommand> _undoStack = new();
		private readonly Stack<IGraphViewCommand> _redoStack = new();

		public void RecordCommand(IGraphViewCommand command)
		{
			_undoStack.Push(command);
			_redoStack.Clear();
		}

		[CanBeNull]
		public IGraphViewCommand Undo()
		{
			if (_undoStack.Count == 0)
				return null;

			var command = _undoStack.Pop();
			_redoStack.Push(command);

			return command;
		}

		[CanBeNull]
		public IGraphViewCommand Redo()
		{
			if (_redoStack.Count == 0)
				return null;

			var command = _redoStack.Pop();
			_undoStack.Push(command);

			return command;
		}
	}
}