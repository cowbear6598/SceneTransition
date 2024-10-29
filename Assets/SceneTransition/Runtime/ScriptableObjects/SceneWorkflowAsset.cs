using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SceneTransition.ScriptableObjects.Data;
using UnityEngine;

namespace SceneTransition.ScriptableObjects
{
	[CreateAssetMenu(fileName = "SceneWorkflowAsset", menuName = "SceneTransition/SceneWorkflowAsset")]
	public class SceneWorkflowAsset : ScriptableObject
	{
		[SerializeReference] private List<OperationData> _operationData = new();

		public async UniTask Execute()
		{
			var workflow = new SceneWorkflow();

			foreach (var data in _operationData)
			{
				var operation = data.CreateOperation();

				workflow.AddOperation(operation);
			}

			await workflow.Execute();
		}

		#if UNITY_EDITOR

		public List<OperationData> OperationData => _operationData;

		public void AddOperation(OperationData operationData)
			=> _operationData.Add(operationData);

		public void ClearOperations() => _operationData.Clear();

		#endif
	}
}