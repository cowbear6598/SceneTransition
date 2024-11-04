using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SceneTransition.Operations
{
	internal class LoadSceneOperation : IOperation
	{
		private readonly AssetReference SceneAsset;
		private readonly float          DelayTime;

		public LoadSceneOperation(AssetReference sceneAsset, float delayTime)
		{
			SceneAsset = sceneAsset;
			DelayTime  = delayTime;
		}

		public async UniTask Execute()
		{
			var sceneInstance = await Addressables.LoadSceneAsync(SceneAsset, LoadSceneMode.Additive).Task;

			await UniTask.Delay(TimeSpan.FromSeconds(DelayTime));

			SceneRepository.Instance.AddLoadedScene(sceneInstance);

			SceneWorkflowEvent.RaiseSceneLoaded(sceneInstance.Scene.name);
		}
	}
}