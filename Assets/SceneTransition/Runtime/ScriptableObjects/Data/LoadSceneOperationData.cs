using System;
using SceneTransition.Operations;
using UnityEngine.AddressableAssets;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class LoadSceneOperationData : OperationData
	{
		public readonly AssetReference SceneAsset;

		public LoadSceneOperationData(string nodeData, AssetReference sceneAsset) : base(OperationType.LoadScene, nodeData)
		{
			SceneAsset = sceneAsset;
		}

		public override IOperation CreateOperation() => new LoadSceneOperation(SceneAsset);
	}
}