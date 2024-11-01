using SceneTransition.Editor.GraphViews.Nodes;
using SceneTransition.ScriptableObjects.Data;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.History.Command
{
	public class ChangePropertyCommand : IGraphViewCommand
	{
		private readonly WorkflowNode  _workflowNode;
		private readonly OperationData _newData;
		private readonly OperationData _oldData;

		public ChangePropertyCommand(WorkflowNode workflowNode, OperationData newData, OperationData oldData)
		{
			_workflowNode = workflowNode;
			_oldData      = oldData;
			_newData      = newData;
		}

		public void Execute(SceneWorkflowGraphView graphView)
		{
			_workflowNode.LoadFromData(_newData);
		}

		public void Undo(SceneWorkflowGraphView graphView)
		{
			_workflowNode.LoadFromData(_oldData);
		}
	}
}