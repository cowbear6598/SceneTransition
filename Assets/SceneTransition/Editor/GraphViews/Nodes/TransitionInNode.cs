using SceneTransition.ScriptableObjects.Data;
using UnityEngine;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionInNode : WorkflowNode
	{
		public GameObject TransitionPrefab;

		public TransitionInNode() : base("轉場進入") { }

		protected override OperationData MakeOperationData(string nodeData) => new TransitionInOperationData(nodeData);
	}
}