using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace SceneTransition.Runtime.Infrastructure
{
	public class SceneRepository
	{
		public static SceneRepository Instance { get; } = new SceneRepository();

		private readonly Stack<SceneInstance> loadedScene = new();

		public void AddLoadedScene(SceneInstance sceneInstance) => loadedScene.Push(sceneInstance);

		public SceneInstance GetLastLoadedScene() => loadedScene.Pop();

		public int LoadedSceneCount => loadedScene.Count;
	}
}