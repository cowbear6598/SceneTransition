using System;
using SceneTransition.Operations;
using SceneTransition.Transition;
using UnityEngine.AddressableAssets;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class TransitionInOperationData : OperationData
	{
		public SceneTransitionBehaviour TransitionPrefab;

		public TransitionInOperationData(string nodeData, SceneTransitionBehaviour transitionPrefab) : base(OperationType.TransitionIn, nodeData)
		{
			TransitionPrefab = transitionPrefab;
		}

		public override IOperation CreateOperation() => new TransitionInOperation(TransitionPrefab);
	}
}