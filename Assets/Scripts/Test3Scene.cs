using SceneTransition.ScriptableObjects;
using UnityEngine;
public class Test3Scene : MonoBehaviour
{
	[SerializeField] private SceneWorkflowAsset _sceneWorkflowAsset;

	public async void Button_TransitionOut() => await _sceneWorkflowAsset.Execute();
}