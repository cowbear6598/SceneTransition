using System;
using System.Linq;
using SceneTransition.ScriptableObjects.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public abstract class WorkflowNode : Node
	{
		public string  Id       { get; private set; }
		public Vector2 Position { get; private set; }

		public readonly Port Input;
		public readonly Port Output;

		private readonly SceneWorkflowGraphView _graphView;

		protected WorkflowNode(string title, SceneWorkflowGraphView graphView)
		{
			Id = Guid.NewGuid().ToString();

			this.title = title;
			_graphView = graphView;

			// 創建輸入端口
			Input          = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
			Input.portName = "執行";
			inputContainer.Add(Input);

			// 創建輸出端口
			Output          = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
			Output.portName = "下一步";
			outputContainer.Add(Output);

			// 添加自定義樣式
			AddToClassList("scene-workflow-node");
			RefreshExpandedState();
			RefreshPorts();
		}

		public void UpdatePosition(Vector2 position)
		{
			Position = position;
		}

		public OperationData CreateOperationData()
		{
			var nodeData = new NodeData
			{
				Id       = Id,
				Position = GetPosition().position,
			};

			if (Input.connected)
			{
				var connections = Input.connections.ToList();
				nodeData.InputNodeId = (connections[0].output.node as WorkflowNode).Id;
			}

			if (Output.connected)
			{
				var connections = Output.connections.ToList();
				nodeData.OutputNodeId = (connections[0].input.node as WorkflowNode).Id;
			}

			return ToOperationData(JsonUtility.ToJson(nodeData));
		}

		protected abstract OperationData ToOperationData(string nodeData);

		public void SetId(string Id) => this.Id = Id;

		public virtual bool IsValidateToSave()                        => true;
		public virtual void LoadFromData(OperationData operationData) { }
	}
}