using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneTransition.Runtime.Domain.Adapters;
using SceneTransition.Runtime.Domain.Operations;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace SceneTransition.Runtime.Domain
{
	public class SceneWorkflow
	{
		private readonly ISceneTransition  _transition;
		private readonly Queue<IOperation> _operations = new();

		public SceneWorkflow(ISceneTransition transition) => _transition = transition;

		private SceneWorkflow AddOperation(IOperation operation)
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