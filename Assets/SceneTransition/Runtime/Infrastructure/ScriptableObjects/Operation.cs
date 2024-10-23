using System;
using UnityEngine.AddressableAssets;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects
{
	[Serializable]
	public class Operation
	{
		public OperationType  Type;
		public AssetReference SceneAsset;
	}
}