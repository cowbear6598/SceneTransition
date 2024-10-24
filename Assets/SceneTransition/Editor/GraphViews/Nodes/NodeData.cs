using System;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	[Serializable]
	public class NodeData
	{
		public string  Id;
		public Vector2 Position;
		public string  InputNodeId;
		public string  OutputNodeId;
	}
}