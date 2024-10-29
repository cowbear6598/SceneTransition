using Cysharp.Threading.Tasks;
using SceneTransition.Transition;
using Object = UnityEngine.Object;

namespace SceneTransition.Operations
{
	public class TransitionInOperation : IOperation
	{
		private readonly SceneTransitionBehaviour transitionPrefab;

		public TransitionInOperation(SceneTransitionBehaviour transitionPrefab) =>
			this.transitionPrefab = transitionPrefab;

		public UniTask Execute()
		{
			var transition = Object.Instantiate(transitionPrefab);

			return transition.TransitionIn();
		}
	}
}