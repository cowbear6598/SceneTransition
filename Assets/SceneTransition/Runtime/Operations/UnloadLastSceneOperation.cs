using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace SceneTransition.Operations
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