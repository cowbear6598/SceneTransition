using SceneTransition.ScriptableObjects.Data;
using SceneTransition.Transition;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SceneTransition.Editor.GraphViews.Nodes
{
	public class TransitionInNode : WorkflowNode
	{
		public SceneTransitionBehaviour TransitionPrefab { get; private set; }

		private readonly ObjectField _objectField;

		public TransitionInNode() : base("轉場進入")
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
					TransitionPrefab = null;

					return;
				}

				var assetPath = AssetDatabase.GetAssetPath(e.newValue);

				TransitionPrefab = e.newValue as SceneTransitionBehaviour;

				if (assetPath.EndsWith(".prefab"))
					return;

				EditorUtility.DisplayDialog("錯誤", $"{e.newValue.name} 不是 Prefab。", "確定");

				_objectField.SetValueWithoutNotify(null);
				TransitionPrefab = null;
			});

			mainContainer.Add(_objectField);
		}

		protected override OperationData MakeOperationData(string nodeData)
			=> new TransitionInOperationData(nodeData, TransitionPrefab);

		public override void LoadFromData(OperationData operationData)
		{
			var data = operationData as TransitionInOperationData;

			TransitionPrefab = data.TransitionPrefab;

			_objectField.SetValueWithoutNotify(TransitionPrefab);
		}

		public override bool IsValidateToSave()
		{
			var isValidate = TransitionPrefab != null;

			if (!isValidate)
				EditorUtility.DisplayDialog("錯誤", "請選擇轉場物件", "確定");

			return isValidate;
		}
	}
}