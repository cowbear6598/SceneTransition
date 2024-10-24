using SceneTransition.Operations;
using SceneTransition.Transition;

namespace SceneTransition.ScriptableObjects.Data
{
	public class TransitionOutOperationData : OperationData
	{
		public ISceneTransition Transitions;

		public TransitionOutOperationData(string nodeData) : base(OperationType.TransitionOut, nodeData) { }

		public override IOperation CreateOperation() => new TransitionOutOperation(Transitions);
	}
}