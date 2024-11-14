using System;

namespace SceneTransition
{
	public static class SceneTransitionEvent
	{
		public static event Action<string> OnSceneLoaded;
		public static event Action<string> OnSceneUnloaded;

		public static event Action OnTransitionInComplete;
		public static event Action OnTransitionOutComplete;

		internal static void RaiseSceneLoaded(string sceneName)
			=> OnSceneLoaded?.Invoke(sceneName);

		internal static void RaiseSceneUnloaded(string sceneName)
			=> OnSceneUnloaded?.Invoke(sceneName);

		internal static void RaiseTransitionInComplete()
			=> OnTransitionInComplete?.Invoke();

		internal static void RaiseTransitionOutComplete()
			=> OnTransitionOutComplete?.Invoke();
	}
}