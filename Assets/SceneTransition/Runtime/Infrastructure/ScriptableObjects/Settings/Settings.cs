using System;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings
{
	[Serializable]
	public abstract class Settings
	{
		public abstract OperationType Type { get; }
	}
}