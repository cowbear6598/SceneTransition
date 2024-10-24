using Cysharp.Threading.Tasks;

namespace SceneTransition.Operations
{
	public interface IOperation
	{
		UniTask Execute();
	}
}