using System;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings
{
	[Serializable]
	public class TransitionOutSettings : Settings
	{
		public override OperationType Type => OperationType.TransitionOut;
	}
}