using System;
using SceneTransition.Operations;
using UnityEngine;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public abstract class OperationData
	{
		public OperationType Type;

		[HideInInspector] public string NodeData;

		protected OperationData(OperationType type, string nodeData)
		{
			Type     = type;
			NodeData = nodeData;
		}

		public abstract IOperation CreateOperation();
	}
}