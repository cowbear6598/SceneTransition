using SceneTransition.ScriptableObjects;
using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField] private SceneWorkflowAsset _workflow1Asset;
	[SerializeField] private SceneWorkflowAsset _workflow2Asset;
	[SerializeField] private SceneWorkflowAsset _workflow3Asset;

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
	}
}