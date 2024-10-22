using Cysharp.Threading.Tasks;

namespace SceneTransition.Runtime.Domain.Adapters
{
	public interface ISceneTransition
	{
		UniTask TransitionIn();
		UniTask TransitionOut();
	}
}