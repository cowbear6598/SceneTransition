using SceneTransition.Operations;
using SceneTransition.Transition;
using UnityEngine.Serialization;

namespace SceneTransition.ScriptableObjects.Data
{
	public class TransitionOutOperationData : OperationData
	{
		public TransitionOutOperationData(string nodeData) : base(OperationType.TransitionOut, nodeData) { }

		public override IOperation CreateOperation() => new TransitionOutOperation();
	}
}