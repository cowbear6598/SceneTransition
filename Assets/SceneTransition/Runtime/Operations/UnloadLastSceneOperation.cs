using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace SceneTransition.Operations
{
	internal class UnloadLastSceneOperation : IOperation
	{
		public async UniTask Execute()
		{
			var sceneInstance = SceneRepository.Instance.GetLastLoadedScene();

			var sceneName = sceneInstance.Scene.name;

			await Addressables.UnloadSceneAsync(sceneInstance).Task;

			SceneWorkflowEvent.RaiseSceneUnloaded(sceneName);
		}
	}
}