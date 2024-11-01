using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SceneTransition.Operations
{
	internal class LoadSceneOperation : IOperation
	{
		private readonly AssetReference sceneAsset;

		public LoadSceneOperation(AssetReference sceneAsset) => this.sceneAsset = sceneAsset;

		public async UniTask Execute()
		{
			var sceneInstance = await Addressables.LoadSceneAsync(sceneAsset, LoadSceneMode.Additive).Task;

			SceneRepository.Instance.AddLoadedScene(sceneInstance);

			SceneWorkflowEvent.RaiseSceneLoaded(sceneInstance.Scene.name);
		}
	}
}