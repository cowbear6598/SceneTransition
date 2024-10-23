using System;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings
{
	[Serializable]
	public class UnloadAllScenesSettings : Settings
	{
		public override OperationType Type => OperationType.UnloadAllScenes;
	}
}