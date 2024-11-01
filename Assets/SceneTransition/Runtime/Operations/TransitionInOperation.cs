using Cysharp.Threading.Tasks;
using SceneTransition.Transition;
using Object = UnityEngine.Object;

namespace SceneTransition.Operations
{
	internal class TransitionInOperation : IOperation
	{
		private readonly SceneTransitionBehaviour transitionPrefab;

		public TransitionInOperation(SceneTransitionBehaviour transitionPrefab) =>
			this.transitionPrefab = transitionPrefab;

		public async UniTask Execute()
		{
			var transition = Object.Instantiate(transitionPrefab);

			await transition.TransitionIn();

			SceneWorkflowEvent.RaiseTransitionInComplete();
		}
	}
}