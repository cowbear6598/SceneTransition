using System;
using SceneTransition.ScriptableObjects.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public abstract class WorkflowNode : Node
	{
		public NodeData NodeData { get; } = new();
		public Port     Input    { get; }
		public Port     Output   { get; }

		protected WorkflowNode(string title)
		{
			NodeData.Id = Guid.NewGuid().ToString();

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

		public abstract OperationData CreateOperationData();
	}
}