using System;
using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.Data;
using SceneTransition.Editor.GraphViews.Nodes;
using SceneTransition.Editor.GraphViews.Nodes.LoadScene;
using SceneTransition.Editor.GraphViews.Nodes.TransitionIn;
using SceneTransition.Editor.GraphViews.Nodes.TransitionOut;
using SceneTransition.Editor.GraphViews.Nodes.UnloadAllScenes;
using SceneTransition.Editor.GraphViews.Nodes.UnloadLastScene;
using SceneTransition.Editor.Windows;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;
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
		private T CreateNode<T>(Vector2 position) where T : Node, new()
		{
			var node = new T();
			node.SetPosition(new Rect(position, Vector2.zero));

			AddElement(node);

			return node;
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

		public void SaveToAsset(SceneWorkflowAsset asset)
		{
			if (!ValidateGraph(out var errorMessage))
				throw new Exception(errorMessage);

			var editorData = new SceneWorkflowEditorData();

			// 儲存節點
			SaveNodeData(editorData);

			// 儲存連接
			SaveConnectionData(editorData);

			var settings = editorData.GenerateSettings();

			asset.SetSettings(settings);

			Debug.Log(JsonUtility.ToJson(editorData));
			asset.SetEditorData(JsonUtility.ToJson(editorData));
		}

		private void SaveNodeData(SceneWorkflowEditorData editorData)
		{
			foreach (var node in nodes.ToList().OfType<WorkflowNode>())
			{
				NodeData nodeData = node switch
				{
					LoadSceneNode loadSceneNode => new LoadSceneNodeData
					{
						Id         = loadSceneNode.NodeId,
						Position   = loadSceneNode.GetPosition().position,
						SceneAsset = loadSceneNode.SceneAsset,
					},

					UnloadAllScenesNode unloadAllScenesNode => new UnloadAllScenesNodeData
					{
						Id       = unloadAllScenesNode.NodeId,
						Position = unloadAllScenesNode.GetPosition().position,
					},

					UnloadLastSceneNode unloadLastSceneNode => new UnloadLastSceneNodeData
					{
						Id       = unloadLastSceneNode.NodeId,
						Position = unloadLastSceneNode.GetPosition().position,
					},

					TransitionInNode transitionInNode => new TransitionInNodeData
					{
						Id       = transitionInNode.NodeId,
						Position = transitionInNode.GetPosition().position,
					},

					TransitionOutNode transitionOutNode => new TransitionOutNodeData
					{
						Id       = transitionOutNode.NodeId,
						Position = transitionOutNode.GetPosition().position,
					},

					_ => null,
				};

				if (nodeData == null)
					throw new Exception("未知的節點類型！");

				editorData.AddNodeData(nodeData);
			}
		}

		private void SaveConnectionData(SceneWorkflowEditorData editorData)
		{
			foreach (var edge in edges)
			{
				if (edge.output.node is not WorkflowNode outputNode ||
				    edge.input.node is not WorkflowNode inputNode)
					continue;

				var outputNodeData = editorData.FindNodeDataById(outputNode.NodeId);
				var inputNodeData  = editorData.FindNodeDataById(inputNode.NodeId);

				if (outputNodeData == null || inputNodeData == null)
					continue;

				outputNodeData.OutputNodeId = inputNode.NodeId;
				inputNodeData.InputNodeId   = outputNode.NodeId;
			}
		}

		// 驗證可否儲存
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

			if (loadSceneNodes.Any(loadNode => loadNode.SceneAsset == null))
			{
				errorMessage = "有 LoadScene 節點未設置場景資源！";

				return false;
			}

			return true;
		}

		public void LoadFromAsset(SceneWorkflowAsset asset)
		{
			var editorData = JsonUtility.FromJson<SceneWorkflowEditorData>(asset.EditorData);

			if (editorData == null)
				throw new Exception("無法讀取編輯器資料！");

			DeleteElements(graphElements.ToList());

			var nodePair = new Dictionary<string, WorkflowNode>();

			foreach (var nodeData in editorData.Nodes)
			{
				WorkflowNode node = nodeData.Type switch
				{
					OperationType.LoadScene       => CreateNode<LoadSceneNode>(nodeData.Position),
					OperationType.UnloadAllScenes => CreateNode<UnloadAllScenesNode>(nodeData.Position),
					OperationType.UnloadLastScene => CreateNode<UnloadLastSceneNode>(nodeData.Position),
					OperationType.TransitionIn    => CreateNode<TransitionInNode>(nodeData.Position),
					OperationType.TransitionOut   => CreateNode<TransitionOutNode>(nodeData.Position),
					_                             => null,
				};

				if (node == null)
					continue;

				node.SetNodeId(nodeData.Id);

				nodePair[nodeData.Id] = node;
			}

			// 重建連接
			foreach (var nodeData in editorData.Nodes)
			{
				if (string.IsNullOrEmpty(nodeData.OutputNodeId)            ||
				    !nodePair.TryGetValue(nodeData.Id, out var outputNode) ||
				    !nodePair.TryGetValue(nodeData.OutputNodeId, out var inputNode)
				   )
					continue;

				var edge = outputNode.Output.ConnectTo(inputNode.Input);
				AddElement(edge);
			}
		}

		#endregion
	}
}