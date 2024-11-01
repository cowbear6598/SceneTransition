using Cysharp.Threading.Tasks;

namespace SceneTransition.Operations
{
	internal class UnloadAllScenesOperation : IOperation
	{
		public async UniTask Execute()
		{
			var count = SceneRepository.Instance.LoadedSceneCount;

			for (var i = 0; i < count; i++)
			{
				var operation = new UnloadLastSceneOperation();

				await operation.Execute();
			}
		}
	}
}