using System;
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

		private readonly ObjectField _objectField;

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
					_transitionPrefab = null;

					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(e.newValue);

				_transitionPrefab = e.newValue as SceneTransitionBehaviour;

				if (assetPath.EndsWith(".prefab"))
					return;

				EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不是 Prefab。", "確定");

				_objectField.SetValueWithoutNotify(null);
				_transitionPrefab = null;
			});

			mainContainer.Add(_objectField);
		}

		protected override OperationData ToOperationData(string nodeData)
			=> new TransitionInOperationData(nodeData, _transitionPrefab);

		public override void LoadFromData(OperationData operationData)
		{
			var data = operationData as TransitionInOperationData;

			_transitionPrefab = data.TransitionPrefab;

			_objectField.SetValueWithoutNotify(_transitionPrefab);
		}

		public override bool IsValidateToSave()
		{
			if (_transitionPrefab != null)
				return true;

			EditorUtility.DisplayDialog("錯誤", "請選擇轉場物件。", "確定");

			return false;
		}
	}
}