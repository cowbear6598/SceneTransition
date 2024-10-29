using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SceneTransition.Transition
{
	public abstract class SceneTransitionBehaviour : MonoBehaviour
	{
		public abstract UniTask TransitionIn();
		public abstract UniTask TransitionOut();
	}
}