using SceneTransition.Editor.GraphViews.History.Command;
using SceneTransition.ScriptableObjects.Data;
using SceneTransition.Transition;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	internal class TransitionInNode : WorkflowNode
	{
		private SceneTransitionBehaviour _transitionPrefab;
		private float                    _delayTime;

		private readonly ObjectField _objectField;
		private readonly FloatField  _delayTimeField;

		public TransitionInNode(SceneWorkflowGraphView graphView) : base("轉場進入", graphView)
		{
			_objectField = new ObjectField("轉換物件")
			{
				objectType        = typeof(SceneTransitionBehaviour),
				allowSceneObjects = false,
				style             = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_objectField.RegisterValueChangedCallback(e =>
			{
				if (e.newValue == null)
				{
					ChangeTransitionPrefab(null);
					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(e.newValue);

				if (!assetPath.EndsWith(".prefab"))
				{
					_objectField.SetValueWithoutNotify(_transitionPrefab);
					EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不是 Prefab。", "確定");
					return;
				}

				ChangeTransitionPrefab(e.newValue as SceneTransitionBehaviour);
			});

			_delayTimeField = new FloatField("等待時間")
			{
				style = { marginTop = 8, marginBottom = 8, marginLeft = 8, marginRight = 8 },
			};

			_delayTimeField.RegisterValueChangedCallback(e =>
			{
				ChangeDelayTime(e.newValue);
			});

			mainContainer.Add(_objectField);
			mainContainer.Add(_delayTimeField);
		}

		private void ChangeDelayTime(float delayTime)
		{
			var oldData = CreateOperationData();

			_delayTime = Mathf.Clamp(delayTime, 0, float.MaxValue);

			var newData = CreateOperationData();

			var command = new ChangePropertyCommand(this, newData, oldData);
			_graphView.ExecuteCommand(command);
		}

		private void ChangeTransitionPrefab(SceneTransitionBehaviour newPrefab)
		{
			var oldData = CreateOperationData();

			_transitionPrefab = newPrefab;

			var newData = CreateOperationData();

			var command = new ChangePropertyCommand(this, newData, oldData);
			_graphView.ExecuteCommand(command);
		}

		protected override OperationData ToOperationData(string nodeData)
			=> new TransitionInOperationData(nodeData, _transitionPrefab, _delayTime);

		internal override void LoadFromData(OperationData operationData)
		{
			var data = operationData as TransitionInOperationData;

			_transitionPrefab = data.TransitionPrefab;
			_delayTime        = data.DelayTime;

			_objectField.SetValueWithoutNotify(_transitionPrefab);
			_delayTimeField.SetValueWithoutNotify(_delayTime);
		}

		internal override bool IsValidateToSave()
		{
			if (_transitionPrefab != null)
				return true;

			EditorUtility.DisplayDialog("錯誤", "請選擇轉場物件。", "確定");

			return false;
		}
	}
}