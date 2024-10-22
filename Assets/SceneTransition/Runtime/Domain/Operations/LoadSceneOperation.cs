using Cysharp.Threading.Tasks;
using SceneTransition.Runtime.Infrastructure;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SceneTransition.Runtime.Domain.Operations
{
	public class LoadSceneOperation : IOperation
	{
		private readonly AssetReference sceneAsset;

		public LoadSceneOperation(AssetReference sceneAsset) => this.sceneAsset = sceneAsset;

		public async UniTask Execute()
		{
			var sceneInstance = await Addressables.LoadSceneAsync(sceneAsset, LoadSceneMode.Additive).Task;

			SceneRepository.Instance.AddLoadedScene(sceneInstance);
		}
	}
}