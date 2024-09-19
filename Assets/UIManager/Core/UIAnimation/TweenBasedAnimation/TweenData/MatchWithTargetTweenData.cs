using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager
{
    [System.Serializable]
    [Flags]
    public enum MatchType
    {
        None = 0,
        Position = 1 << 0,
        Rotation = 1 << 1,
        Scale = 1 << 2,
        Color = 1 << 3,
        Alpha = 1 << 4
    }

    public class MatchWithTargetTweenData : BaseTweenData
    {

        public override TweenDataType DataType => TweenDataType.Match;

        public MatchType matchType = MatchType.Position | MatchType.Rotation | MatchType.Scale | MatchType.Color | MatchType.Alpha;

        public UIElement targetElement; // Reference to the target UI element

        

        public override Tween CreateTweenImplementation(UIElement element)
        {
            Sequence sequence = DOTween.Sequence();

            if ((matchType & MatchType.Position) == MatchType.Position)
            {
                Vector3 targetPosition = targetElement.transform.position;
                sequence.Join(element.transform.DOMove(targetPosition, Duration).SetEase(Ease));
            }

            if ((matchType & MatchType.Rotation) == MatchType.Rotation)
            {
                Quaternion targetRotation = targetElement.transform.rotation;
                sequence.Join(element.transform.DORotateQuaternion(targetRotation, Duration).SetEase(Ease));
            }

            if ((matchType & MatchType.Scale) == MatchType.Scale)
            {
                Vector3 targetScale = targetElement.transform.localScale;
                sequence.Join(element.transform.DOScale(targetScale, Duration).SetEase(Ease));
            }

            if ((matchType & MatchType.Color) == MatchType.Color)
            {
                Graphic graphic = element.GetComponent<Graphic>();
                Graphic targetGraphic = targetElement.GetComponent<Graphic>();
                if (graphic != null && targetGraphic != null)
                {
                    Color targetColor = targetGraphic.color;
                    sequence.Join(graphic.DOColor(targetColor, Duration).SetEase(Ease));
                }
            }

            if ((matchType & MatchType.Alpha) == MatchType.Alpha)
            {
                CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
                CanvasGroup targetCanvasGroup = targetElement.GetComponent<CanvasGroup>();
                if (canvasGroup != null && targetCanvasGroup != null)
                {
                    float targetAlpha = targetCanvasGroup.alpha;
                    sequence.Join(canvasGroup.DOFade(targetAlpha, Duration).SetEase(Ease));
                }
            }

            if (Loop)
            {
                sequence.SetLoops(LoopCount, LoopType);
            }
            
            return sequence;
        }
    }
}
