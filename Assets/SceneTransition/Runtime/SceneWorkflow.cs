using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneTransition.Operations;
using SceneTransition.Transition;
using UnityEngine.AddressableAssets;

namespace SceneTransition
{
	public class SceneWorkflow
	{
		private readonly ISceneTransition  _transition;
		private readonly Queue<IOperation> _operations = new();

		public SceneWorkflow(ISceneTransition transition) => _transition = transition;

		public SceneWorkflow AddOperation(IOperation operation)
		{
			_operations.Enqueue(operation);

			return this;
		}

		#region Operations

		public SceneWorkflow TransitionIn()                  => AddOperation(new TransitionInOperation(_transition));
		public SceneWorkflow TransitionOut()                 => AddOperation(new TransitionOutOperation(_transition));
		public SceneWorkflow LoadScene(AssetReference scene) => AddOperation(new LoadSceneOperation(scene));
		public SceneWorkflow UnloadAllScenes()               => AddOperation(new UnloadAllScenesOperation());
		public SceneWorkflow UnloadLastScene()               => AddOperation(new UnloadLastSceneOperation());

		#endregion

		public async UniTask Execute()
		{
			foreach (var operation in _operations)
				await operation.Execute();
		}
	}
}