using Cysharp.Threading.Tasks;

namespace SceneTransition.Runtime.Domain.Operations
{
	public interface IOperation
	{
		UniTask Execute();
	}
}