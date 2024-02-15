using DG.Tweening;
using UnityEngine;

namespace UI_Manager
{
    [System.Serializable]
    public class FadeTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Fade;
        public enum FadeFrom { InitialAlpha, CurrentAlpha, SpecificAlpha, OtherObjectAlpha }
        public enum FadeTo { InitialAlpha, CurrentAlpha, SpecificAlpha, OtherObjectAlpha }

        public FadeFrom fadeFrom;
        public FadeTo fadeTo;

        [ShowInEnum(nameof(fadeFrom), nameof(FadeFrom.SpecificAlpha))]
        public float startAlpha;

        [ShowInEnum(nameof(fadeTo), nameof(FadeTo.SpecificAlpha))]
        public float endAlpha;

        [ShowInEnum(nameof(fadeFrom), nameof(FadeFrom.OtherObjectAlpha))]
        public CanvasGroup startOtherCanvasGroup;

        [ShowInEnum(nameof(fadeTo), nameof(FadeTo.OtherObjectAlpha))]
        public CanvasGroup endOtherCanvasGroup;

        public override Tween CreateTweenImplementation(UI_Element element)
        {
            CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = element.gameObject.AddComponent<CanvasGroup>();
            }

            float startAlphaValue = 1.0f;
            float endAlphaValue = 1.0f;

            // Handle start alpha
            switch (fadeFrom)
            {
                case FadeFrom.InitialAlpha:
                    startAlphaValue = element.ElementInfo.Value.firstAlpha;
                    break;
                case FadeFrom.CurrentAlpha:
                    startAlphaValue = canvasGroup.alpha;
                    break;
                case FadeFrom.SpecificAlpha:
                    startAlphaValue = startAlpha;
                    break;
                case FadeFrom.OtherObjectAlpha:
                    startAlphaValue = startOtherCanvasGroup != null ? startOtherCanvasGroup.alpha : 1.0f;
                    break;
            }

            // Handle end alpha
            switch (fadeTo)
            {
                case FadeTo.InitialAlpha:
                    endAlphaValue = element.ElementInfo.Value.firstAlpha;
                    break;
                case FadeTo.CurrentAlpha:
                    endAlphaValue = canvasGroup.alpha;
                    break;
                case FadeTo.SpecificAlpha:
                    endAlphaValue = endAlpha;
                    break;
                case FadeTo.OtherObjectAlpha:
                    endAlphaValue = endOtherCanvasGroup != null ? endOtherCanvasGroup.alpha : 1.0f;
                    break;
            }

            // Create the fade tween
            canvasGroup.alpha = startAlphaValue;
            Tween tween = canvasGroup.DOFade(endAlphaValue, duration).SetEase(ease).SetLoops(loopCount, loopType);

            return tween;
        }
    }
}
