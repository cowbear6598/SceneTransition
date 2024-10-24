using System;
using SceneTransition.Operations;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class UnloadLastSceneOperationData : OperationData
	{
		public UnloadLastSceneOperationData(string nodeData) : base(OperationType.UnloadLastScene, nodeData) { }

		public override IOperation CreateOperation() => new UnloadLastSceneOperation();
	}
}