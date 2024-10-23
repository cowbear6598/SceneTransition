using System.Collections.Generic;
using SceneTransition.Runtime.Domain;
using SceneTransition.Runtime.Domain.Adapters;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;
using UnityEngine;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects
{
	[CreateAssetMenu(fileName = "SceneWorkflowAsset", menuName = "SceneTransition/SceneWorkflowAsset")]
	public class SceneWorkflowAsset : ScriptableObject
	{
		[SerializeField] private string _editorData;

		[SerializeField] private List<Settings.Settings> _settings = new();

		public SceneWorkflow CreateWorkflow(ISceneTransition transition)
		{
			var workflow = new SceneWorkflow(transition);

			foreach (var settings in _settings)
			{
				switch (settings)
				{
					case LoadSceneSettings loadSceneSettings:
						workflow.LoadScene(loadSceneSettings.SceneAsset);

						break;
					case UnloadAllScenesSettings unloadAllScenesSettings:
						workflow.UnloadAllScenes();

						break;
					case UnloadLastSceneSettings unloadLastSceneSettings:
						workflow.UnloadLastScene();

						break;

					case TransitionInSettings transitionInSettings:
						workflow.TransitionIn();

						break;
					case TransitionOutSettings transitionOutSettings:
						workflow.TransitionOut();

						break;
				}
			}

			return workflow;
		}

#if UNITY_EDITOR
		public string EditorData => _editorData;

		public void SetEditorData(string editorData)
			=> _editorData = editorData;

		public void SetSettings(List<Settings.Settings> operations)
			=> _settings = operations;
#endif
	}
}