using System;
using SceneTransition.Operations;

namespace SceneTransition.ScriptableObjects.Data
{
	[Serializable]
	public class DelayOperationData : OperationData
	{
		public float DelayTime;

		public DelayOperationData(string nodeData, float delayTime) : base(OperationType.Delay, nodeData)
		{
			DelayTime = delayTime;
		}

		public override IOperation CreateOperation() => new DelayOperation(DelayTime);
	}
}