using System;
using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.GraphViews.Nodes;
using SceneTransition.Editor.Windows;
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
		private readonly SceneWorkflowEditorWindow _editorWindow;

		public SceneWorkflowGraphView(SceneWorkflowEditorWindow editorWindow)
		{
			_editorWindow = editorWindow;

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

		private T CreateNode<T>(Vector2 position) where T : WorkflowNode, new()
		{
			var node = new T();
			node.SetPosition(new Rect(position, Vector2.zero));

			AddElement(node);

			return node;
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

		#region 儲存/載入

		public void SaveToAsset(SceneWorkflowAsset asset)
		{
			if (!ValidateSave(out var errorMessage))
				throw new Exception(errorMessage);

			// 儲存節點資料
			SaveNodeData();

			// 儲存至 Asset
			ApplyToAsset(asset);
		}

		private void SaveNodeData()
		{
			var workflowNodes = nodes.ToList().OfType<WorkflowNode>().ToList();

			foreach (var node in workflowNodes)
			{
				node.NodeData.Position = node.GetPosition().position;
			}

			foreach (var edge in edges)
			{
				if (edge.output.node is not WorkflowNode outputNode ||
				    edge.input.node is not WorkflowNode inputNode)
					continue;

				outputNode.NodeData.OutputNodeId = inputNode.NodeData.Id;
				inputNode.NodeData.InputNodeId   = outputNode.NodeData.Id;
			}
		}

		private void ApplyToAsset(SceneWorkflowAsset asset)
		{
			var workflowNodes = nodes.ToList().OfType<WorkflowNode>().ToList();
			var startNode     = workflowNodes.First(node => !node.Input.connected);

			var currentNode = startNode;

			while (currentNode != null)
			{
				asset.AddOperation(currentNode.CreateOperationData());
				currentNode = workflowNodes.FirstOrDefault(n => n.NodeData.Id == currentNode.NodeData.OutputNodeId);
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

			// 檢查讀取場景有沒有放入場景資源
			if (nodes.OfType<LoadSceneNode>().Any(n => n.SceneAsset == null))
			{
				errorMessage = "有 LoadScene 節點未設置場景資源！";

				return false;
			}

			return true;
		}

		public void LoadFromAsset(SceneWorkflowAsset asset)
		{
			DeleteElements(graphElements.ToList());

			var nodeIdToInstance = new Dictionary<string, WorkflowNode>();

			foreach (var operationData in asset.OperationData)
			{
				var nodeData = JsonUtility.FromJson<NodeData>(operationData.NodeData);

				WorkflowNode node = operationData.Type switch
				{
					OperationType.LoadScene       => CreateNode<LoadSceneNode>(nodeData.Position),
					OperationType.UnloadAllScenes => CreateNode<UnloadAllScenesNode>(nodeData.Position),
					OperationType.UnloadLastScene => CreateNode<UnloadLastSceneNode>(nodeData.Position),
					OperationType.TransitionIn    => CreateNode<TransitionInNode>(nodeData.Position),
					OperationType.TransitionOut   => CreateNode<TransitionOutNode>(nodeData.Position),
					_                             => throw new Exception("未知的操作類型！"),
				};

				node.NodeData.Id              = nodeData.Id;
				nodeIdToInstance[nodeData.Id] = node;

				if (node is LoadSceneNode loadSceneNode)
				{
					loadSceneNode.SetSceneAssetByLoad((operationData as LoadSceneOperationData)?.SceneAsset);
				}
			}

			foreach (var operationData in asset.OperationData)
			{
				var nodeData = JsonUtility.FromJson<NodeData>(operationData.NodeData);

				if (string.IsNullOrEmpty(nodeData.OutputNodeId)                    ||
				    !nodeIdToInstance.TryGetValue(nodeData.Id, out var outputNode) ||
				    !nodeIdToInstance.TryGetValue(nodeData.OutputNodeId, out var inputNode)
				   )
					continue;

				var edge = outputNode.Output.ConnectTo(inputNode.Input);
				AddElement(edge);
			}
		}

		#endregion
	}
}