using SceneTransition.ScriptableObjects;
using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField] private SceneWorkflowAsset _workflowAsset;

	private async void Awake()
	{
		await _workflowAsset.Execute();
	}
}