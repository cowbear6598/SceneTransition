using System;
using SceneTransition.Operations;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class UnloadAllScenesOperationData : OperationData
	{
		public UnloadAllScenesOperationData(string nodeData) : base(OperationType.UnloadAllScenes, nodeData) { }
		public override IOperation CreateOperation() => new UnloadAllScenesOperation();
	}
}