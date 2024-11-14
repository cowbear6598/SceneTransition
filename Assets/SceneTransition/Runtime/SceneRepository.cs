using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace SceneTransition
{
	internal class SceneRepository
	{
		public static SceneRepository Instance { get; } = new();

		private readonly Stack<string> loadedScene = new();

		public void AddLoadedScene(string scene)
		{
			loadedScene.Push(scene);
		}

		public string GetLastLoadedScene() => loadedScene.Pop();

		public int LoadedSceneCount => loadedScene.Count;
	}
}