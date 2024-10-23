using System;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	[Serializable]
	public abstract class NodeData
	{
		[field: SerializeField] public string  Id;
		public                         Vector2 Position;
		public                         string  InputNodeId;
		public                         string  OutputNodeId;

		public abstract OperationType Type { get; }
		public abstract Settings      ToSettings();
	}
}