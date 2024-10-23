using System;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings
{
	[Serializable]
	public class UnloadLastSceneSettings : Settings
	{
		public override OperationType Type => OperationType.UnloadLastScene;
	}
}