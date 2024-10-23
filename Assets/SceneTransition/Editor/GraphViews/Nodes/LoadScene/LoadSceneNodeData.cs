using System;
using SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings;
using UnityEngine.AddressableAssets;

namespace SceneTransition.Editor.GraphViews.Nodes.LoadScene
{
	[Serializable]
	public class LoadSceneNodeData : NodeData
	{
		public override OperationType Type         => OperationType.LoadScene;
		public override Settings      ToSettings() => new LoadSceneSettings { SceneAsset = SceneAsset };

		public AssetReference SceneAsset;
	}
}