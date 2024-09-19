using DG.Tweening;
using EditorTools;
using UnityEngine;

namespace UIManager
{
    [System.Serializable]
    public class FadeTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Fade;

        [field: SerializeField] public FadeFrom FadeFrom { get; private set; }
        [field: SerializeField] public FadeTo FadeTo { get; private set; }


        [field: SerializeField, ShowInEnum(nameof(FadeFrom), nameof(FadeFrom.SpecificAlpha))]
        public float StartAlpha { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(FadeFrom), nameof(FadeFrom.OtherObjectAlpha))]
        public CanvasGroup StartOtherCanvasGroup { get; private set; }



        [field: SerializeField, ShowInEnum(nameof(FadeTo), nameof(FadeTo.SpecificAlpha))]
        public float EndAlpha { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(FadeTo), nameof(FadeTo.OtherObjectAlpha))]
        public CanvasGroup EndOtherCanvasGroup { get; private set; }


        public override Tween CreateTweenImplementation(UIElement element)
        {
            if (!element.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup = element.gameObject.AddComponent<CanvasGroup>();
            }

            float startAlphaValue = 1.0f;
            float endAlphaValue = 1.0f;

            // Handle start alpha
            switch (FadeFrom)
            {
                case FadeFrom.InitialAlpha:
                    startAlphaValue = element.ElementInfo.Value.firstAlpha;
                    break;
                case FadeFrom.CurrentAlpha:
                    startAlphaValue = canvasGroup.alpha;
                    break;
                case FadeFrom.SpecificAlpha:
                    startAlphaValue = StartAlpha;
                    break;
                case FadeFrom.OtherObjectAlpha:
                    startAlphaValue = StartOtherCanvasGroup != null ? StartOtherCanvasGroup.alpha : 1.0f;
                    break;
            }

            // Handle end alpha
            switch (FadeTo)
            {
                case FadeTo.InitialAlpha:
                    endAlphaValue = element.ElementInfo.Value.firstAlpha;
                    break;
                case FadeTo.CurrentAlpha:
                    endAlphaValue = canvasGroup.alpha;
                    break;
                case FadeTo.SpecificAlpha:
                    endAlphaValue = EndAlpha;
                    break;
                case FadeTo.OtherObjectAlpha:
                    endAlphaValue = EndOtherCanvasGroup != null ? EndOtherCanvasGroup.alpha : 1.0f;
                    break;
            }

            // Create the fade tween
            canvasGroup.alpha = startAlphaValue;
            Tween tween;
            if (Loop)
            {
                tween = canvasGroup.DOFade(endAlphaValue, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType);
            }
            else
            {
                tween = canvasGroup.DOFade(endAlphaValue, Duration).SetEase(Ease);
            }

            return tween;
        }
    }


    public enum FadeFrom { InitialAlpha, CurrentAlpha, SpecificAlpha, OtherObjectAlpha }
    public enum FadeTo { InitialAlpha, CurrentAlpha, SpecificAlpha, OtherObjectAlpha }

}
