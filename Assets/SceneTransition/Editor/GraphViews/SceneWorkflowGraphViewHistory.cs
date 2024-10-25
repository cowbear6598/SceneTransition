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
		}

		[CanBeNull]
		public IGraphViewCommand Undo()
		{
			if (_undoStack.Count == 0)
				return null;

			var command = _undoStack.Pop();
			_redoStack.Push(command);

			if (_redoStack.Count > 10)
				_redoStack.Pop();

			return command;
		}

		[CanBeNull]
		public IGraphViewCommand Redo()
		{
			if (_redoStack.Count == 0)
				return null;

			var command = _redoStack.Pop();
			_undoStack.Push(command);

			if (_undoStack.Count > 10)
				_undoStack.Pop();

			return command;
		}
	}
}