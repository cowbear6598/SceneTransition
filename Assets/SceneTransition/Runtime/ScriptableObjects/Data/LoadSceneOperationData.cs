using System;
using SceneTransition.Operations;
using UnityEngine.AddressableAssets;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class LoadSceneOperationData : OperationData
	{
		public AssetReference SceneAsset;
		public float          DelayTime;

		public LoadSceneOperationData(string nodeData, AssetReference sceneAsset, float delayTime) : base(OperationType.LoadScene, nodeData)
		{
			SceneAsset = sceneAsset;
			DelayTime  = delayTime;
		}

		public override IOperation CreateOperation() => new LoadSceneOperation(SceneAsset, DelayTime);
	}
}