using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	[System.Serializable]
	public class LoadSceneNodeData : ScriptableObject
	{
		public string         NodeId;
		public Vector2        Position;
		public string         Title;
		public AssetReference SceneReference;
	}
}