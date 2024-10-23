using System;
using UnityEngine.AddressableAssets;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings
{
	[Serializable]
	public class LoadSceneSettings : Settings
	{
		public override OperationType Type => OperationType.LoadScene;

		public AssetReference SceneAsset;
	}
}