using System;
using SceneTransition.Operations;
using SceneTransition.Transition;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class TransitionInOperationData : OperationData
	{
		public SceneTransitionBehaviour TransitionPrefab;
		public float                    DelayTime;

		public TransitionInOperationData(string nodeData, SceneTransitionBehaviour transitionPrefab, float delayTime) : base(OperationType.TransitionIn, nodeData)
		{
			TransitionPrefab = transitionPrefab;
			DelayTime        = delayTime;
		}

		public override IOperation CreateOperation() => new TransitionInOperation(TransitionPrefab, DelayTime);
	}
}