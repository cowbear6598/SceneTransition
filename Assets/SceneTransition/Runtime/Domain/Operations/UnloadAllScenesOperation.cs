using Cysharp.Threading.Tasks;
using SceneTransition.Runtime.Infrastructure;

namespace SceneTransition.Runtime.Domain.Operations
{
	public class UnloadAllScenesOperation : IOperation
	{
		public async UniTask Execute()
		{
			for (var i = 0; i < SceneRepository.Instance.LoadedSceneCount; i++)
			{
				var unloadLastSceneOperation = new UnloadLastSceneOperation();

				await unloadLastSceneOperation.Execute();
			}
		}
	}
}