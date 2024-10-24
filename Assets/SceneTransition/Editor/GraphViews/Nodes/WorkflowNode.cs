﻿using SceneTransition.Operations;
using SceneTransition.ScriptableObjects.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public abstract class WorkflowNode : Node
	{
		public NodeData NodeData { get; private set; } = new();
		public Port     Input    { get; private set; }
		public Port     Output   { get; private set; }

		public abstract OperationType OperationType { get; }

		protected WorkflowNode(string title)
		{
			NodeData.Id = System.Guid.NewGuid().ToString();

			this.title = title;

			// 創建輸入端口
			Input          = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
			Input.portName = "執行";
			inputContainer.Add(Input);

			// 創建輸出端口
			Output          = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
			Output.portName = "下一步";
			outputContainer.Add(Output);

			// 添加自定義樣式
			AddToClassList("scene-workflow-node");
			RefreshExpandedState();
			RefreshPorts();
		}

		public abstract OperationData CreateOperationData();
	}
}