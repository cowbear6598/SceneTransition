﻿using System.Collections.Generic;
using JetBrains.Annotations;
using SceneTransition.Editor.GraphViews.History.Command;

namespace SceneTransition.Editor.GraphViews.History
{
	public class SceneWorkflowGraphViewHistory
	{
		private const int MAX_OPERATIONS = 20;

		private readonly LinkedList<IGraphViewCommand> _undoList = new();
		private readonly LinkedList<IGraphViewCommand> _redoList = new();

		public void RecordCommand(IGraphViewCommand command)
		{
			_undoList.AddLast(command);

			if (_undoList.Count > MAX_OPERATIONS)
				_undoList.RemoveFirst();
		}

		[CanBeNull]
		public IGraphViewCommand Undo()
		{
			if (_undoList.Count == 0)
				return null;

			var command = _undoList.Last.Value;
			_undoList.RemoveLast();
			_redoList.AddLast(command);

			if (_redoList.Count > MAX_OPERATIONS)
				_redoList.RemoveFirst();

			return command;
		}

		[CanBeNull]
		public IGraphViewCommand Redo()
		{
			if (_redoList.Count == 0)
				return null;

			var command = _redoList.Last.Value;
			_redoList.RemoveLast();
			_undoList.AddLast(command);

			if (_undoList.Count > MAX_OPERATIONS)
				_undoList.RemoveFirst();

			return command;
		}

		public void ClearRedo()
		{
			_redoList.Clear();
		}
	}
}