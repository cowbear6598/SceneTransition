using UnityEditor.Experimental.GraphView;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public abstract class WorkflowNode : Node
	{
		public string NodeId { get; private set; }
		public Port   Input  { get; private set; }
		public Port   Output { get; private set; }

		protected WorkflowNode(string title)
		{
			NodeId = System.Guid.NewGuid().ToString();

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
	}
}