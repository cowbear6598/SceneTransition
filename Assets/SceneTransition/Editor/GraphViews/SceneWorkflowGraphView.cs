using System.Collections.Generic;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews
{
	public class SceneWorkflowGraphView : GraphView
	{
		public SceneWorkflowGraphView()
		{
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
			this.AddManipulator(new ContentZoomer());
			this.AddManipulator(CreateLoadSceneManipulator());

			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			style.flexGrow   = 1;
			style.flexShrink = 1;
		}

		private IManipulator CreateLoadSceneManipulator()
		{
			var manipulator = new ContextualMenuManipulator(
				menuEvent => menuEvent.menu.AppendAction(
					"新增/讀取場景",
					actionEvent => CreateLoadSceneNode(actionEvent.eventInfo.localMousePosition)
				)
			);

			return manipulator;
		}

		private void CreateLoadSceneNode(Vector2 position)
		{
			var node = new LoadSceneNode();
			node.SetPosition(new Rect(position, Vector2.zero));

			AddElement(node);
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			var compatiblePorts = new List<Port>();

			ports.ForEach(port =>
			{
				if (startPort.node == port.node)
					return;

				if (startPort.direction == port.direction)
					return;

				compatiblePorts.Add(port);
			});

			return compatiblePorts;
		}
	}
}