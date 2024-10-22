using Cysharp.Threading.Tasks;
using SceneTransition.Runtime.Infrastructure;
using UnityEngine.AddressableAssets;

namespace SceneTransition.Runtime.Domain.Operations
{
	public class UnloadLastSceneOperation : IOperation
	{
		public async UniTask Execute()
		{
			var sceneInstance = SceneRepository.Instance.GetLastLoadedScene();

			await Addressables.UnloadSceneAsync(sceneInstance).Task;
		}
	}
}