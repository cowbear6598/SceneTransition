using SceneTransition.ScriptableObjects.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class DelayNode : WorkflowNode
	{
		private float _delayTime;

		private readonly FloatField _floatField;

		public DelayNode() : base("等待")
		{
			_floatField = new FloatField("等待時間")
			{
				style = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_floatField.RegisterValueChangedCallback(e =>
			{
				_delayTime = Mathf.Clamp(e.newValue, 0, float.MaxValue);

				_floatField.SetValueWithoutNotify(_delayTime);
			});

			mainContainer.Add(_floatField);
		}

		public override void LoadFromData(OperationData operationData)
		{
			var data = operationData as DelayOperationData;

			_delayTime = data.DelayTime;

			_floatField.SetValueWithoutNotify(_delayTime);
		}

		protected override OperationData ToOperationData(string nodeData) => new DelayOperationData(nodeData, _delayTime);
	}
}