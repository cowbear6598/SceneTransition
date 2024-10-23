using System;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes.Data
{
	[Serializable]
	public class NodeData
	{
		public string        Id;
		public OperationType Type;
		public Vector2       Position;
		public string        InputNodeId;
		public string        OutputNodeId;
		public string        SerializedData;
	}
}