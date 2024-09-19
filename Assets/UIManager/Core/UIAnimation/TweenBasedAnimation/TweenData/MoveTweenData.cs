using DG.Tweening;
using EditorTools;
using UnityEngine;

namespace UIManager
{
    [System.Serializable]
    public class MoveTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Move;


        [field: SerializeField] public Space Space { get; private set; }
        [field: SerializeField] public MoveFrom MoveFrom { get; private set; }
        [field: SerializeField] public MoveTo MoveTo { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(MoveFrom), nameof(MoveFrom.Position))]
        public Vector3 StartPosition { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(MoveFrom), nameof(MoveFrom.OtherTransformPosition))]
        public Transform StartTransform { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(MoveFrom), nameof(MoveFrom.DeltaPositionFromFirstPosition), nameof(MoveFrom.DeltaPositionFromCurrentPosition))]
        public Vector3 StartDeltaPosition { get; private set; }



        [field: SerializeField, ShowInEnum(nameof(MoveTo), nameof(MoveTo.Position))]
        public Vector3 EndPosition { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(MoveTo), nameof(MoveTo.OtherTransformPosition))]
        public Transform EndTransform { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(MoveTo), nameof(MoveTo.DeltaPositionFromFirstPosition), nameof(MoveTo.DeltaPositionFromCurrentPosition))]
        public Vector3 EndDeltaPosition { get; private set; }

        public override Tween CreateTweenImplementation(UIElement element)
        {
            Tween tween = null;
            Vector3 posStart = Vector3.zero;
            Vector3 posEnd = Vector3.zero;

            // Handle start position
            switch (MoveFrom)
            {
                case MoveFrom.FirstPosition:
                    posStart = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos);
                    break;
                case MoveFrom.CurrentPosition:
                    posStart = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition);
                    break;
                case MoveFrom.Position:
                    posStart = StartPosition;
                    break;
                case MoveFrom.DeltaPositionFromFirstPosition:
                    posStart = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos) + StartDeltaPosition;
                    break;
                case MoveFrom.DeltaPositionFromCurrentPosition:
                    posStart = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition) + StartDeltaPosition;
                    break;
                case MoveFrom.OtherTransformPosition:
                    posStart = StartTransform.position;
                    break;
            }

            // Handle end position
            switch (MoveTo)
            {
                case MoveTo.FirstPosition:
                    posEnd = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos);
                    break;
                case MoveTo.CurrentPosition:
                    posEnd = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition);
                    break;
                case MoveTo.Position:
                    posEnd = EndPosition;
                    break;
                case MoveTo.DeltaPositionFromFirstPosition:
                    posEnd = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos) + EndDeltaPosition;
                    break;
                case MoveTo.DeltaPositionFromCurrentPosition:
                    posEnd = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition) + EndDeltaPosition;
                    break;
                case MoveTo.OtherTransformPosition:
                    posEnd = EndTransform.position;
                    break;
            }

            // Create the tween
            tween = CreatePositionTween(element, posStart, posEnd);

            return tween;
        }

        private Vector3 GetPositionFromSpace(UIElement element, Vector3 localPosition, Vector3 worldPosition, Vector3 anchorPosition)
        {
            return Space switch
            {
                Space.Local => localPosition,
                Space.World => worldPosition,
                Space.Anchor => anchorPosition,
                _ => Vector3.zero,
            };
        }

        private Tween CreatePositionTween(UIElement element, Vector3 startPos, Vector3 endPos)
        {
            switch (Space)
            {
                case Space.Local:
                    element.transform.localPosition = startPos;
                    return Loop ? element.transform.DOLocalMove(endPos, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType) :
                        element.transform.DOLocalMove(endPos, Duration).SetEase(Ease);
                case Space.World:
                    element.transform.position = startPos;
                    return Loop ? element.transform.DOMove(endPos, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType) :
                        element.transform.DOMove(endPos, Duration).SetEase(Ease);
                case Space.Anchor:
                    element.RectTransform.anchoredPosition = startPos;
                    return Loop ? element.RectTransform.DOAnchorPos(endPos, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType) :
                        element.RectTransform.DOAnchorPos(endPos, Duration).SetEase(Ease);
                default:
                    return null;
            }
        }
    }

    public enum MoveFrom { FirstPosition, CurrentPosition, Position, DeltaPositionFromFirstPosition, DeltaPositionFromCurrentPosition, OtherTransformPosition }
    public enum MoveTo { FirstPosition, CurrentPosition, Position, DeltaPositionFromFirstPosition, DeltaPositionFromCurrentPosition, OtherTransformPosition }
    public enum Space { Local, World, Anchor }
}
