using SceneTransition.Editor.GraphViews.Nodes;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Manipulators
{
	public class NodeSelector : MouseManipulator
	{
		private readonly SceneWorkflowGraphView _graphView;
		private readonly WorkflowNode           _node;

		public NodeSelector(SceneWorkflowGraphView graphView, WorkflowNode node)
		{
			_graphView = graphView;
			_node      = node;

			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
		}

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<MouseDownEvent>(OnMouseDown);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
		}

		private void OnMouseDown(MouseDownEvent e)
		{
			_graphView.StartDragNode(_node);
		}
	}
}