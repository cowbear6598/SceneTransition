using System;
using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.GraphViews.History;
using SceneTransition.Editor.GraphViews.History.Command;
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
		private readonly WorkflowNodeFactory           _factory;

		public bool IsDirty { get; private set; }

		private Action<bool> _onDirtyChanged;

		public SceneWorkflowGraphView()
		{
			_factory = new WorkflowNodeFactory(this);

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

		#region 初始化

		// 新增樣式
		private void AddStyles()
		{
			var guids = AssetDatabase.FindAssets("SceneWorkflowGraphViewCss t:StyleSheet");

			var path       = AssetDatabase.GUIDToAssetPath(guids[0]);
			var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);

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
					"場景/讀取場景",
					action => DropdownCreateNode(action, OperationType.LoadScene)
				);

				menuEvent.menu.AppendAction(
					"場景/卸載所有場景",
					action => DropdownCreateNode(action, OperationType.UnloadAllScenes)
				);

				menuEvent.menu.AppendAction(
					"場景/卸載上一個場景",
					action => DropdownCreateNode(action, OperationType.UnloadLastScene)
				);

				menuEvent.menu.AppendAction(
					"轉場/進入",
					action => DropdownCreateNode(action, OperationType.TransitionIn)
				);

				menuEvent.menu.AppendAction(
					"轉場/退出",
					action => DropdownCreateNode(action, OperationType.TransitionOut)
				);

				menuEvent.menu.AppendAction(
					"等待",
					action => DropdownCreateNode(action, OperationType.Delay)
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

		private void DropdownCreateNode(DropdownMenuAction action, OperationType type)
		{
			var mousePosition = GetMousePositionInWorldSpace(action.eventInfo.localMousePosition);

			AddNode(type, mousePosition);
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

		internal void ExecuteCommand(IGraphViewCommand command)
		{
			command.Execute(this);
			_history.RecordCommand(command);
			_history.ClearRedo();

			SetDirty(true);
		}

		public void Undo()
		{
			var command = _history.Undo();

			command?.Undo(this);

			SetDirty(true);
		}

		public void Redo()
		{
			var command = _history.Redo();

			command?.Execute(this);

			SetDirty(true);
		}

		#endregion

		#region 節點建立

		private void AddNode(OperationType type, Vector2 position)
		{
			var node = _factory.CreateNode(type, position);

			var command = new AddNodeCommand(node, position);
			ExecuteCommand(command);
		}

		private void AddNodeByOperationData(List<OperationData> operationData)
		{
			var nodeIdToInstance = new Dictionary<string, WorkflowNode>();

			foreach (var data in operationData)
			{
				var nodeData = JsonUtility.FromJson<NodeData>(data.NodeData);

				var node = _factory.CreateNode(data.Type, nodeData.Position);

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

		#endregion

		#region 儲存/載入

		public void SaveToAsset(SceneWorkflowAsset asset)
		{
			if (!ValidateSave())
				return;

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

			SetDirty(false);
		}

		// 驗證可否儲存
		private bool ValidateSave()
		{
			var workflowNodes = nodes.ToList().OfType<WorkflowNode>().ToList();

			if (workflowNodes.Count == 0)
			{
				EditorUtility.DisplayDialog("儲存失敗", "請建立節點後再進行儲存！", "確定");

				return false;
			}

			// 檢查起/終點是否只有一個
			var startNodes = workflowNodes.Count(node => !node.Input.connected);
			var endNodes   = workflowNodes.Count(node => !node.Output.connected);

			if (startNodes != 1)
			{
				var errorMessage = startNodes == 0 ? "請確保有起點！" : "請確保只有一個起點！";

				EditorUtility.DisplayDialog("儲存失敗", errorMessage, "確定");

				return false;
			}

			if (endNodes != 1)
			{
				var errorMessage = endNodes == 0 ? "請確保有終點！" : "請確保只有一個終點！";

				EditorUtility.DisplayDialog("儲存失敗", errorMessage, "確定");

				return false;
			}

			return workflowNodes.All(node => node.IsValidateToSave());
		}

		public void LoadFromAsset(SceneWorkflowAsset asset)
		{
			DeleteElements(graphElements.ToList());
			AddNodeByOperationData(asset.OperationData);
		}

		#endregion

		private void SetDirty(bool isDirty)
		{
			IsDirty = isDirty;

			_onDirtyChanged?.Invoke(IsDirty);
		}

		public void RegisterOnDirtyChanged(Action<bool> onDirtyChanged)
			=> _onDirtyChanged += onDirtyChanged;
	}
}