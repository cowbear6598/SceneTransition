using SceneTransition;
using SceneTransition.ScriptableObjects;
using UnityEngine;

public class TestScene : MonoBehaviour
{
	[SerializeField] private SceneWorkflowAsset _workflow1Asset;
	[SerializeField] private SceneWorkflowAsset _workflow2Asset;
	[SerializeField] private SceneWorkflowAsset _workflow3Asset;
	[SerializeField] private SceneWorkflowAsset _workflow4Asset;
	[SerializeField] private SceneWorkflowAsset _workflow5Asset;

	private void OnEnable()
	{
		SceneTransitionEvent.OnSceneLoaded           += OnSceneLoaded;
		SceneTransitionEvent.OnSceneUnloaded         += OnSceneUnloaded;
		SceneTransitionEvent.OnTransitionInComplete  += OnTransitionInComplete;
		SceneTransitionEvent.OnTransitionOutComplete += OnTransitionOutComplete;
	}

	private void OnDisable()
	{
		SceneTransitionEvent.OnSceneLoaded           -= OnSceneLoaded;
		SceneTransitionEvent.OnSceneUnloaded         -= OnSceneUnloaded;
		SceneTransitionEvent.OnTransitionInComplete  -= OnTransitionInComplete;
		SceneTransitionEvent.OnTransitionOutComplete -= OnTransitionOutComplete;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			_workflow1Asset.Execute();
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			_workflow2Asset.Execute();
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			_workflow3Asset.Execute();
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			_workflow4Asset.Execute();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			_workflow5Asset.Execute();
		}
	}

	private void OnSceneLoaded(string sceneName)
	{
		Debug.Log($"Scene Loaded: {sceneName}");
	}

	private void OnSceneUnloaded(string sceneName)
	{
		Debug.Log($"Scene Unloaded: {sceneName}");
	}

	private void OnTransitionInComplete()
	{
		Debug.Log("Transition In Complete");
	}

	private void OnTransitionOutComplete()
	{
		Debug.Log("Transition Out Complete");
	}
}