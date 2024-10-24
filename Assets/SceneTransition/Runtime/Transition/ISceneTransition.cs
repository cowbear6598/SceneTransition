using Cysharp.Threading.Tasks;

namespace SceneTransition.Transition
{
	public interface ISceneTransition
	{
		UniTask TransitionIn();
		UniTask TransitionOut();
	}
}