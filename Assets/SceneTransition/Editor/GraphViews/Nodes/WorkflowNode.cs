using System;
using System.Linq;
using SceneTransition.Operations;
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

		protected WorkflowNode(string title)
		{
			Id = Guid.NewGuid().ToString();

			this.title = title;

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

			return MakeOperationData(JsonUtility.ToJson(nodeData));
		}

		protected abstract OperationData MakeOperationData(string nodeData);

		public void SetId(string Id) => this.Id = Id;

		public virtual bool IsValidateToSave() => true;
	}
}