using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace SceneTransition.Operations
{
	internal class LoadSceneOperation : IOperation
	{
		private readonly string SceneName;
		private readonly float  DelayTime;

		public LoadSceneOperation(string sceneName, float delayTime)
		{
			SceneName = sceneName;
			DelayTime = delayTime;
		}

		public async UniTask Execute()
		{
			await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

			await UniTask.Delay(TimeSpan.FromSeconds(DelayTime));

			SceneRepository.Instance.AddLoadedScene(SceneName);

			SceneTransitionEvent.RaiseSceneLoaded(SceneName);
		}
	}
}