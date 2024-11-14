using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace SceneTransition.Operations
{
	internal class UnloadLastSceneOperation : IOperation
	{
		public async UniTask Execute()
		{
			var sceneName = SceneRepository.Instance.GetLastLoadedScene();

			await SceneManager.UnloadSceneAsync(sceneName);

			SceneTransitionEvent.RaiseSceneUnloaded(sceneName);
		}
	}
}