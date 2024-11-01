using System;
using SceneTransition.Operations;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class WorkflowNodeFactory
	{
		private readonly SceneWorkflowGraphView _graphView;

		public WorkflowNodeFactory(SceneWorkflowGraphView graphView) => _graphView = graphView;

		internal WorkflowNode CreateNode(OperationType type, Vector2 position)
		{
			WorkflowNode node = type switch
			{
				OperationType.LoadScene       => new LoadSceneNode(_graphView),
				OperationType.UnloadAllScenes => new UnloadAllScenesNode(_graphView),
				OperationType.UnloadLastScene => new UnloadLastSceneNode(_graphView),
				OperationType.TransitionIn    => new TransitionInNode(_graphView),
				OperationType.TransitionOut   => new TransitionOutNode(_graphView),
				OperationType.Delay           => new DelayNode(_graphView),
				_                             => throw new Exception("未知的操作類型！"),
			};

			node.UpdatePosition(position);

			return node;
		}
	}
}