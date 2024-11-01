using System;
using Cysharp.Threading.Tasks;

namespace SceneTransition.Operations
{
	public class DelayOperation : IOperation
	{
		private readonly float _delayTime;

		public DelayOperation(float delayTime) => _delayTime = delayTime;

		public async UniTask Execute()
			=> await UniTask.Delay(TimeSpan.FromSeconds(_delayTime));
	}
}