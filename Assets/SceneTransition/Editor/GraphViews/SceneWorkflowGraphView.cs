using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.GraphViews.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews
{
	public class SceneWorkflowGraphView : GraphView
	{
		public SceneWorkflowGraphView()
		{
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
			this.AddManipulator(new ContentZoomer());

			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			AddStyles();
			AddNodeContextMenu();
		}

		#region 初始化

		// 新增樣式
		private void AddStyles()
		{
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/SceneTransition/Editor/GraphViews/SceneWorkflowGraphViewCss.uss");

			styleSheets.Add(styleSheet);

			style.flexGrow   = 1;
			style.flexShrink = 1;

			AddToClassList("workflow-graph-view");
		}

		// 新增節點右鍵選單
		private void AddNodeContextMenu()
		{
			var manipulator = new ContextualMenuManipulator(menuEvent =>
			{
				menuEvent.menu.AppendAction(
					"新增/讀取場景",
					actionEvent => CreateNode<LoadSceneNode>(GetCorrectMousePosition(actionEvent.eventInfo.localMousePosition))
				);

				menuEvent.menu.AppendAction(
					"新增/卸載所有場景",
					actionEvent => CreateNode<UnloadAllScenesNode>(GetCorrectMousePosition(actionEvent.eventInfo.localMousePosition))
				);

				menuEvent.menu.AppendAction(
					"轉場/進入",
					actionEvent => CreateNode<TransitionInNode>(GetCorrectMousePosition(actionEvent.eventInfo.localMousePosition))
				);

				menuEvent.menu.AppendAction(
					"轉場/退出",
					actionEvent => CreateNode<TransitionOutNode>(GetCorrectMousePosition(actionEvent.eventInfo.localMousePosition))
				);
			});

			this.AddManipulator(manipulator);
		}

		// 取得正確的滑鼠位置
		private Vector2 GetCorrectMousePosition(Vector2 mousePosition)
		{
			Vector2 viewPosition = viewTransform.matrix.inverse.MultiplyPoint3x4(mousePosition);

			return viewPosition;
		}

		// 建立節點
		private void CreateNode<T>(Vector2 position) where T : Node, new()
		{
			var node = new T();
			node.SetPosition(new Rect(position, Vector2.zero));

			AddElement(node);
		}

		// 連接節點
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

		#endregion

		#region 儲存/載入

		// 驗證可否儲存

		public void SaveToAsset()
		{
			if (!ValidateGraph(out var errorMessage))
			{
				EditorUtility.DisplayDialog("儲存失敗", errorMessage, "確定");

				return;
			}
		}

		private bool ValidateGraph(out string errorMessage)
		{
			errorMessage = string.Empty;

			var nodes = this.nodes.ToList().OfType<WorkflowNode>().ToList();

			if (nodes.Count == 0)
			{
				errorMessage = "請建立節點後再進行儲存！";

				return false;
			}

			// 檢查起點是否只有一個
			var startNodes = nodes.Where(
				node => !node.Input.connected
			).ToList();

			if (startNodes.Count != 1)
			{
				errorMessage = "請確保只有一個起點！";

				return false;
			}

			if (startNodes.Count == 0)
			{
				errorMessage = "請確保有起點！";

				return false;
			}

			// 檢查終點是否只有一個
			var endNodes = nodes.Where(
				node => !node.Output.connected
			).ToList();

			if (endNodes.Count != 1)
			{
				errorMessage = "請確保只有一個終點！";

				return false;
			}

			if (endNodes.Count == 0)
			{
				errorMessage = "請確保有終點！";

				return false;
			}

			// 檢查讀取場景有沒有放入場景資源
			var loadSceneNodes = nodes.OfType<LoadSceneNode>();

			if (loadSceneNodes.Any(loadNode => loadNode.SceneReference == null))
			{
				errorMessage = "有 LoadScene 節點未設置場景資源！";

				return false;
			}

			return true;
		}

		#endregion
	}
}