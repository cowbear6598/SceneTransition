using Cysharp.Threading.Tasks;
using PrimeTween;
using SceneTransition.Transition;
using UnityEngine;

public class FadeSceneTransition : SceneTransitionBehaviour
{
	[SerializeField] private CanvasGroup _canvasGroup;

	private async UniTask SetAppear(bool isAppear)
	{
		var duration = 0.5f;

		_canvasGroup.blocksRaycasts = isAppear;
		_canvasGroup.interactable   = isAppear;

		await Tween.Alpha(_canvasGroup, isAppear ? 0 : 1, isAppear ? 1 : 0, duration);
	}

	public override UniTask TransitionIn()  => SetAppear(true);
	public override UniTask TransitionOut() => SetAppear(false);
}