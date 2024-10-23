using System.Collections.Generic;
using SceneTransition.Runtime.Domain;
using SceneTransition.Runtime.Domain.Adapters;
using UnityEngine;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects
{
	[CreateAssetMenu(fileName = "SceneWorkflowAsset", menuName = "SceneTransition/SceneWorkflowAsset")]
	public class SceneWorkflowAsset : ScriptableObject
	{
		[SerializeField, HideInInspector] private string _editorData;

		[SerializeField] private List<Operation> _operations = new();

		public SceneWorkflow CreateWorkflow(ISceneTransition transition)
		{
			var workflow = new SceneWorkflow(transition);

			foreach (var operation in _operations)
			{
				switch (operation.Type)
				{
					case OperationType.LoadScene:
						workflow.LoadScene(operation.SceneAsset);

						break;
					case OperationType.UnloadAllScenes:
						workflow.UnloadAllScenes();

						break;
					case OperationType.TransitionIn:
						workflow.TransitionIn();

						break;
					case OperationType.TransitionOut:
						workflow.TransitionOut();

						break;
				}
			}

			return workflow;
		}

#if UNITY_EDITOR
		public string EditorData {
			get => _editorData;
			set => _editorData = value;
		}

		public void SetOperations(List<Operation> operations)
			=> this._operations = operations;
#endif
	}
}