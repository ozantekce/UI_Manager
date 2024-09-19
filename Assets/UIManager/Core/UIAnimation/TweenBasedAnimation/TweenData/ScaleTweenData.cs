using DG.Tweening;
using EditorTools;
using UnityEngine;

namespace UIManager
{
    [System.Serializable]
    public class ScaleTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Scale;


        [field: SerializeField] public ScaleFrom ScaleFrom { get; private set; }
        [field: SerializeField] public ScaleTo ScaleTo { get; private set; }

        [ShowInEnum(nameof(ScaleFrom), nameof(ScaleFrom.Scale))]
        public Vector3 startScale;

        [ShowInEnum(nameof(ScaleFrom), nameof(ScaleFrom.OtherTransformScale))]
        public Transform startTransform;

        [ShowInEnum(nameof(ScaleFrom), nameof(ScaleFrom.DeltaScaleFromFirstScale), nameof(ScaleFrom.DeltaScaleFromCurrentScale))]
        public Vector3 startDeltaScale;

        [ShowInEnum(nameof(ScaleTo), nameof(ScaleTo.Scale))]
        public Vector3 endScale;

        [ShowInEnum(nameof(ScaleTo), nameof(ScaleTo.OtherTransformScale))]
        public Transform endTransform;

        [ShowInEnum(nameof(ScaleTo), nameof(ScaleTo.DeltaScaleFromFirstScale), nameof(ScaleTo.DeltaScaleFromCurrentScale))]
        public Vector3 endDeltaScale;

        public override Tween CreateTweenImplementation(UIElement element)
        {
            Tween tween = null;
            Vector3 scaleStart = Vector3.zero;
            Vector3 scaleEnd = Vector3.zero;

            // Handle start scale
            switch (ScaleFrom)
            {
                case ScaleFrom.FirstScale:
                    scaleStart = element.ElementInfo.Value.firstLocalScale;
                    break;
                case ScaleFrom.CurrentScale:
                    scaleStart = element.transform.localScale;
                    break;
                case ScaleFrom.Scale:
                    scaleStart = startScale;
                    break;
                case ScaleFrom.DeltaScaleFromFirstScale:
                    scaleStart = element.ElementInfo.Value.firstLocalScale + startDeltaScale;
                    break;
                case ScaleFrom.DeltaScaleFromCurrentScale:
                    scaleStart = element.transform.localScale + startDeltaScale;
                    break;
                case ScaleFrom.OtherTransformScale:
                    scaleStart = startTransform.localScale;
                    break;
            }

            // Handle end scale
            switch (ScaleTo)
            {
                case ScaleTo.FirstScale:
                    scaleEnd = element.ElementInfo.Value.firstLocalScale;
                    break;
                case ScaleTo.CurrentScale:
                    scaleEnd = element.transform.localScale;
                    break;
                case ScaleTo.Scale:
                    scaleEnd = endScale;
                    break;
                case ScaleTo.DeltaScaleFromFirstScale:
                    scaleEnd = element.ElementInfo.Value.firstLocalScale + endDeltaScale;
                    break;
                case ScaleTo.DeltaScaleFromCurrentScale:
                    scaleEnd = element.transform.localScale + endDeltaScale;
                    break;
                case ScaleTo.OtherTransformScale:
                    scaleEnd = endTransform.localScale;
                    break;
            }

            // Create the tween
            element.transform.localScale = scaleStart;

            if (Loop)
            {
                tween = element.transform.DOScale(scaleEnd, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType);
            }
            else
            {
                tween = element.transform.DOScale(scaleEnd, Duration).SetEase(Ease);
            }

            return tween;
        }
    }

    public enum ScaleFrom { FirstScale, CurrentScale, Scale, DeltaScaleFromFirstScale, DeltaScaleFromCurrentScale, OtherTransformScale }
    public enum ScaleTo { FirstScale, CurrentScale, Scale, DeltaScaleFromFirstScale, DeltaScaleFromCurrentScale, OtherTransformScale }
}
