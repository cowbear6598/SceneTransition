using System;
using SceneTransition.Operations;
using SceneTransition.Transition;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class TransitionInOperationData : OperationData
	{
		public ISceneTransition Transition;

		public TransitionInOperationData(string nodeData) : base(OperationType.TransitionIn, nodeData) { }

		public override IOperation CreateOperation() => new TransitionInOperation(Transition);
	}
}