using System;
using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.GraphViews.Command;
using SceneTransition.Editor.GraphViews.Nodes;
using SceneTransition.Operations;
using SceneTransition.ScriptableObjects;
using SceneTransition.ScriptableObjects.Data;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews
{
	public class SceneWorkflowGraphView : GraphView
	{
		private readonly SceneWorkflowGraphViewHistory _history = new();

		private Vector2?     _dragStartPosition;
		private WorkflowNode _draggingNode;

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

			graphViewChanged += OnGraphViewChanged;
		}

		#region UI 操作紀錄

		private GraphViewChange OnGraphViewChanged(GraphViewChange change)
		{
			HandleMoveNodes(change);
			HandleRemoveNodes(change);
			HandleCreateEdge(change);
			HandleRemoveEdges(change);

			return change;
		}

		private void HandleMoveNodes(GraphViewChange change)
		{
			if (change.movedElements == null)
				return;

			var nodesToMove = change.movedElements.OfType<WorkflowNode>().ToList();

			var oldPositions = nodesToMove.Select(node => node.Position).ToList();
			var newPositions = nodesToMove.Select(node => node.GetPosition().position).ToList();

			var command = new MoveNodeCommand(nodesToMove, oldPositions, newPositions);
			ExecuteCommand(command);
		}

		private void HandleRemoveNodes(GraphViewChange change)
		{
			if (change.elementsToRemove == null)
				return;

			var workflowNodes = change.elementsToRemove.OfType<WorkflowNode>().ToList();

			if (workflowNodes.Count == 0)
				return;

			var command = new RemoveNodesCommand(workflowNodes);
			ExecuteCommand(command);
		}

		private void HandleCreateEdge(GraphViewChange change)
		{
			if (change.edgesToCreate == null)
				return;

			var edgeToCreate = change.edgesToCreate.First();

			var command = new AddEdgeCommand(edgeToCreate);
			ExecuteCommand(command);
		}

		private void HandleRemoveEdges(GraphViewChange change)
		{
			if (change.elementsToRemove == null)
				return;

			var edgesToRemove = change.elementsToRemove.OfType<Edge>().ToList();

			if (edgesToRemove.Count == 0)
				return;

			var command = new RemoveEdgesCommand(edgesToRemove);
			ExecuteCommand(command);
		}

		#endregion

		#region 歷史紀錄與執行

		private void ExecuteCommand(IGraphViewCommand command)
		{
			command.Execute(this);
			_history.RecordCommand(command);
			_history.ClearRedo();
		}

		public void Undo()
		{
			var command = _history.Undo();

			command?.Undo(this);
		}

		public void Redo()
		{
			var command = _history.Redo();

			command?.Execute(this);
		}

		#endregion

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
				menuEvent.menu.ClearItems();

				menuEvent.menu.AppendAction(
					"新增/讀取場景",
					DropdownCreateNode<LoadSceneNode>
				);

				menuEvent.menu.AppendAction(
					"新增/卸載所有場景",
					DropdownCreateNode<UnloadAllScenesNode>
				);

				menuEvent.menu.AppendAction(
					"轉場/進入",
					DropdownCreateNode<TransitionInNode>
				);

				menuEvent.menu.AppendAction(
					"轉場/退出",
					DropdownCreateNode<TransitionOutNode>
				);
			});

			this.AddManipulator(manipulator);
		}

		// 取得縮放後的滑鼠位置
		private Vector2 GetMousePositionInWorldSpace(Vector2 mousePosition)
		{
			Vector2 viewPosition = viewTransform.matrix.inverse.MultiplyPoint3x4(mousePosition);

			return viewPosition;
		}

		private void DropdownCreateNode<T>(DropdownMenuAction action) where T : WorkflowNode, new()
		{
			var mousePosition = GetMousePositionInWorldSpace(action.eventInfo.localMousePosition);

			CreateNode<T>(mousePosition);
		}

		// 定義節點可連接的 Port
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

		#region 節點建立

		private T CreateNode<T>(Vector2 position) where T : WorkflowNode, new()
		{
			var node = new T();

			var command = new AddNodeCommand(node, position);
			ExecuteCommand(command);

			node.UpdatePosition(position);

			return node;
		}

		private void CreateNodes(List<OperationData> operationData)
		{
			var nodeIdToInstance = new Dictionary<string, WorkflowNode>();

			foreach (var data in operationData)
			{
				var nodeData = JsonUtility.FromJson<NodeData>(data.NodeData);

				var node = CreateNodeByType(data, nodeData);

				nodeIdToInstance[nodeData.Id] = node;

				node.SetId(nodeData.Id);
				node.LoadFromData(data);
			}

			foreach (var data in operationData)
			{
				var nodeData = JsonUtility.FromJson<NodeData>(data.NodeData);

				if (string.IsNullOrEmpty(nodeData.OutputNodeId))
					continue;

				var inputNode  = nodeIdToInstance[nodeData.Id];
				var outputNode = nodeIdToInstance[nodeData.OutputNodeId];

				var edge = inputNode.Output.ConnectTo(outputNode.Input);
				AddElement(edge);
			}
		}

		private WorkflowNode CreateNodeByType(OperationData data, NodeData nodeData)
		{
			WorkflowNode node = data.Type switch
			{
				OperationType.LoadScene       => CreateNode<LoadSceneNode>(nodeData.Position),
				OperationType.UnloadAllScenes => CreateNode<UnloadAllScenesNode>(nodeData.Position),
				OperationType.UnloadLastScene => CreateNode<UnloadLastSceneNode>(nodeData.Position),
				OperationType.TransitionIn    => CreateNode<TransitionInNode>(nodeData.Position),
				OperationType.TransitionOut   => CreateNode<TransitionOutNode>(nodeData.Position),
				_                             => throw new Exception("未知的操作類型！"),
			};
			return node;
		}

		#endregion

		#region 儲存/載入

		public void SaveToAsset(SceneWorkflowAsset asset)
		{
			if (!ValidateSave(out var errorMessage))
				throw new Exception(errorMessage);

			// 儲存至 Asset
			var workflowNodes = nodes.ToList().OfType<WorkflowNode>().ToList();

			var startNode = workflowNodes.First(node => !node.Input.connected);

			var currentNode = startNode;

			while (currentNode != null)
			{
				asset.AddOperation(currentNode.CreateOperationData());

				currentNode = currentNode.Output.connected
					? currentNode.Output.connections.First().input.node as WorkflowNode
					: null;
			}
		}

		// 驗證可否儲存
		private bool ValidateSave(out string errorMessage)
		{
			errorMessage = string.Empty;

			var nodes = this.nodes.ToList().OfType<WorkflowNode>().ToList();

			if (nodes.Count == 0)
			{
				errorMessage = "請建立節點後再進行儲存！";

				return false;
			}

			// 檢查起/終點是否只有一個
			var startNodes = nodes.Count(node => !node.Input.connected);
			var endNodes   = nodes.Count(node => !node.Output.connected);

			if (startNodes != 1)
			{
				errorMessage = startNodes == 0 ? "請確保有起點！" : "請確保只有一個起點！";

				return false;
			}

			if (endNodes != 1)
			{
				errorMessage = endNodes == 0 ? "請確保有終點！" : "請確保只有一個終點！";

				return false;
			}

			return nodes.All(node => node.IsValidateToSave());
		}

		public void LoadFromAsset(SceneWorkflowAsset asset)
		{
			DeleteElements(graphElements.ToList());
			CreateNodes(asset.OperationData);
		}

		#endregion
	}
}