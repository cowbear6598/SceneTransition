using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.GraphViews.Nodes;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;

namespace SceneTransition.Editor.Data
{
	[Serializable]
	public class SceneWorkflowEditorData
	{
		public List<NodeData> Nodes = new();

		private List<NodeData> SortNodesByExecution()
		{
			var sorted = new List<NodeData>();

			var startNode = Nodes.First(node => string.IsNullOrEmpty(node.InputNodeId));

			var currentNode = startNode;

			while (currentNode != null)
			{
				sorted.Add(currentNode);

				currentNode = !string.IsNullOrEmpty(currentNode.OutputNodeId)
					? Nodes.Find(n => n.Id == currentNode.OutputNodeId)
					: null;
			}

			return sorted;
		}

		public List<Settings> GenerateSettings()
		{
			try
			{
				var sortedNodes = SortNodesByExecution();

				return sortedNodes.Select(node => node.ToSettings()).ToList();
			}
			catch (Exception e)
			{
				throw new Exception($"生成操作序列時發生錯誤：{e.Message}");
			}
		}

		public void     AddNodeData(NodeData    nodeData) => Nodes.Add(nodeData);
		public NodeData FindNodeDataById(string nodeId)   => Nodes.Find(n => n.Id == nodeId);
	}
}