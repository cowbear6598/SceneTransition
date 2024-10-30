using System;
using Cysharp.Threading.Tasks;
using SceneTransition.Transition;
using Object = UnityEngine.Object;

namespace SceneTransition.Operations
{
	public class TransitionOutOperation : IOperation
	{
		public async UniTask Execute()
		{
			var transition = Object.FindFirstObjectByType<SceneTransitionBehaviour>();

			if (transition == null)
				throw new InvalidOperationException("沒有指定 ISceneTransition 實例。");

			await transition.TransitionOut();

			Object.Destroy(transition.gameObject);
		}
	}
}