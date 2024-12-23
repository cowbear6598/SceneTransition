﻿using SceneTransition.Editor.GraphViews.History.Command;
using SceneTransition.ScriptableObjects.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class DelayNode : WorkflowNode
	{
		private float _delayTime;

		private readonly FloatField _floatField;

		public DelayNode(SceneWorkflowGraphView graphView) : base("等待", graphView)
		{
			_floatField = new FloatField("等待時間")
			{
				style = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_floatField.RegisterValueChangedCallback(e =>
			{
				ChangeDelayTime(e.newValue);
			});

			mainContainer.Add(_floatField);
		}

		private void ChangeDelayTime(float delayTime)
		{
			var oldData = CreateOperationData();

			_delayTime = Mathf.Clamp(delayTime, 0, float.MaxValue);

			var newData = CreateOperationData();

			var command = new ChangePropertyCommand(this, newData, oldData);
			_graphView.ExecuteCommand(command);
		}

		internal override void LoadFromData(OperationData operationData)
		{
			var data = operationData as DelayOperationData;

			_delayTime = data.DelayTime;

			_floatField.SetValueWithoutNotify(_delayTime);
		}

		protected override OperationData ToOperationData(string nodeData) => new DelayOperationData(nodeData, _delayTime);
	}
}