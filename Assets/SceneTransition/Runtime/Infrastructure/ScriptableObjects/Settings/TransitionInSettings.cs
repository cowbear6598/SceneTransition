﻿using System;

namespace SceneTransition.Runtime.Infrastructure.ScriptableObjects.Settings
{
	[Serializable]
	public class TransitionInSettings : Settings
	{
		public override OperationType Type => OperationType.TransitionIn;
	}
}