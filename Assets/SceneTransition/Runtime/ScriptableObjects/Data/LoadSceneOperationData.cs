using System;
using SceneTransition.Operations;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class LoadSceneOperationData : OperationData
	{
		public string SceneName;
		public float  DelayTime;

		public LoadSceneOperationData(string nodeData, string sceneName, float delayTime) : base(OperationType.LoadScene, nodeData)
		{
			SceneName = sceneName;
			DelayTime = delayTime;
		}

		public override IOperation CreateOperation() => new LoadSceneOperation(SceneName, DelayTime);
	}
}