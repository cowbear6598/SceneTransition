using System;
using Cysharp.Threading.Tasks;
using SceneTransition.Transition;
using Object = UnityEngine.Object;

namespace SceneTransition.Operations
{
	internal class TransitionInOperation : IOperation
	{
		private readonly SceneTransitionBehaviour TransitionPrefab;
		private readonly float                    DelayTime;

		public TransitionInOperation(SceneTransitionBehaviour transitionPrefab, float delayTime)
		{
			TransitionPrefab = transitionPrefab;
			DelayTime        = delayTime;
		}

		public async UniTask Execute()
		{
			var transition = Object.Instantiate(TransitionPrefab);

			await transition.TransitionIn();

			await UniTask.Delay(TimeSpan.FromSeconds(DelayTime));

			SceneWorkflowEvent.RaiseTransitionInComplete();
		}
	}
}