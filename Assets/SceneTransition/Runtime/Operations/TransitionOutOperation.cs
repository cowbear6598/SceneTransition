using System;
using Cysharp.Threading.Tasks;
using SceneTransition.Transition;

namespace SceneTransition.Operations
{
	public class TransitionOutOperation : IOperation
	{
		private readonly ISceneTransition transition;

		public TransitionOutOperation(ISceneTransition transition) => this.transition = transition;

		public UniTask Execute()
		{
			if (transition == null)
				throw new InvalidOperationException("沒有指定 ISceneTransition 實例。");

			return transition.TransitionOut();
		}
	}
}