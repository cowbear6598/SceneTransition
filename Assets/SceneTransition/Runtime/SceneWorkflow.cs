using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneTransition.Operations;

namespace SceneTransition
{
	public class SceneWorkflow
	{
		private readonly Queue<IOperation> _operations = new();

		public void AddOperation(IOperation operation)
		{
			_operations.Enqueue(operation);
		}

		public async UniTask Execute()
		{
			foreach (var operation in _operations)
				await operation.Execute();
		}
	}
}