using System;
using System.Collections.Generic;
using System.Linq;
using SceneTransition.Editor.GraphViews.Nodes.Data;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects;
using UnityEngine;

namespace SceneTransition.Editor.Data
{
	[Serializable]
	public class SceneWorkflowEditorData
	{
		public List<NodeData> Nodes = new();

		public void SaveNodeData<T>(NodeData node, T data) where T : class
		{
			node.SerializedData = JsonUtility.ToJson(data);
		}

		public T LoadNodeData<T>(NodeData node) where T : class
		{
			if (!string.IsNullOrEmpty(node.SerializedData))
				return JsonUtility.FromJson<T>(node.SerializedData);

			Debug.LogError("無法讀取節點資料：SerializedData 為空");

			return null;
		}

		private List<NodeData> SortNodesByExecution()
		{
			var sorted   = new List<NodeData>();
			var visited  = new HashSet<string>();
			var visiting = new HashSet<string>();

			// 找出起始節點
			var startNodes = Nodes.Where(
				node => string.IsNullOrEmpty(node.InputNodeId)
			).ToList();

			foreach (var node in startNodes)
			{
				if (!visited.Contains(node.Id))
				{
					if (!TopologicalSort(node, visited, visiting, sorted))
					{
						throw new Exception("檢測到循環依賴！");
					}
				}
			}

			sorted.Reverse();

			if (sorted.Count != Nodes.Count)
			{
				throw new Exception("工作流程圖中存在孤立的節點！");
			}

			return sorted;
		}

		private bool TopologicalSort(
			NodeData        node,
			HashSet<string> visited,
			HashSet<string> visiting,
			List<NodeData>  sorted)
		{
			if (visiting.Contains(node.Id))
				return false;

			if (visited.Contains(node.Id))
				return true;

			visiting.Add(node.Id);

			if (!string.IsNullOrEmpty(node.OutputNodeId))
			{
				var outputNode = Nodes.Find(n => n.Id == node.OutputNodeId);

				if (outputNode != null && !TopologicalSort(outputNode, visited, visiting, sorted))
				{
					return false;
				}
			}

			visiting.Remove(node.Id);
			visited.Add(node.Id);
			sorted.Add(node);

			return true;
		}

		public List<Operation> GenerateOperations()
		{
			try
			{
				var sortedNodes = SortNodesByExecution();
				var operations  = new List<Operation>();

				foreach (var node in sortedNodes)
				{
					var operation = new Operation { Type = node.Type };

					if (node.Type == OperationType.LoadScene)
					{
						var loadSceneData = LoadNodeData<LoadSceneNodeData>(node);

						if (loadSceneData != null)
						{
							operation.SceneAsset = loadSceneData.SceneAsset;
						}
					}

					operations.Add(operation);
				}

				return operations;
			}
			catch (Exception e)
			{
				Debug.LogError($"生成操作序列時發生錯誤：{e.Message}");

				return new List<Operation>();
			}
		}
	}
}