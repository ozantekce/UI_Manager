using DG.Tweening;
using EditorTools;
using UnityEngine;

namespace UIManager
{
    [System.Serializable]
    public class RotateTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Rotate;


        [field: SerializeField] public RotateSpace Space { get; private set; }
        [field: SerializeField] public RotateFrom RotateFrom { get; private set; }
        [field: SerializeField] public RotateTo RotateTo { get; private set; }


        [field: SerializeField, ShowInEnum(nameof(RotateFrom), nameof(RotateFrom.Rotation))]
        public Vector3 StartRotation { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(RotateFrom), nameof(RotateFrom.OtherTransformRotation))]
        public Transform StartTransform { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(RotateFrom), nameof(RotateFrom.DeltaRotation))]
        public Vector3 StartDeltaRotation { get; private set; }



        [field: SerializeField, ShowInEnum(nameof(RotateTo), nameof(RotateTo.Rotation))]
        public Vector3 EndRotation { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(RotateTo), nameof(RotateTo.OtherTransformRotation))]
        public Transform EndTransform { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(RotateTo), nameof(RotateTo.DeltaRotation))]
        public Vector3 EndDeltaRotation { get; private set; }

        public override Tween CreateTweenImplementation(UIElement element)
        {
            Tween tween = null;
            Quaternion rotStart = Quaternion.identity;
            Quaternion rotEnd = Quaternion.identity;

            // Handle start rotation
            switch (RotateFrom)
            {
                case RotateFrom.FirstRotation:
                    rotStart = Space == RotateSpace.Local ? element.ElementInfo.Value.firstLocalRot : element.ElementInfo.Value.firstGlobalRot;
                    break;
                case RotateFrom.CurrentRotation:
                    rotStart = Space == RotateSpace.Local ? element.transform.localRotation : element.transform.rotation;
                    break;
                case RotateFrom.Rotation:
                    rotStart = Quaternion.Euler(StartRotation);
                    break;
                case RotateFrom.DeltaRotation:
                    rotStart = (Space == RotateSpace.Local ? element.transform.localRotation : element.transform.rotation) * Quaternion.Euler(StartDeltaRotation);
                    break;
                case RotateFrom.OtherTransformRotation:
                    rotStart = StartTransform.rotation;
                    break;
            }

            // Handle end rotation
            switch (RotateTo)
            {
                case RotateTo.FirstRotation:
                    rotEnd = Space == RotateSpace.Local ? element.ElementInfo.Value.firstLocalRot : element.ElementInfo.Value.firstGlobalRot;
                    break;
                case RotateTo.CurrentRotation:
                    rotEnd = Space == RotateSpace.Local ? element.transform.localRotation : element.transform.rotation;
                    break;
                case RotateTo.Rotation:
                    rotEnd = Quaternion.Euler(EndRotation);
                    break;
                case RotateTo.DeltaRotation:
                    rotEnd = (Space == RotateSpace.Local ? element.transform.localRotation : element.transform.rotation) * Quaternion.Euler(EndDeltaRotation);
                    break;
                case RotateTo.OtherTransformRotation:
                    rotEnd = EndTransform.rotation;
                    break;
            }

            // Create the tween
            if (Space == RotateSpace.Local)
            {
                element.transform.localRotation = rotStart;
                tween = Loop ? element.transform.DOLocalRotateQuaternion(rotEnd, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType) :
                    element.transform.DOLocalRotateQuaternion(rotEnd, Duration).SetEase(Ease);
            }
            else if (Space == RotateSpace.World)
            {
                element.transform.rotation = rotStart;
                tween = Loop ? element.transform.DORotateQuaternion(rotEnd, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType) :
                    element.transform.DORotateQuaternion(rotEnd, Duration).SetEase(Ease);
            }

            return tween;
        }
    }

    public enum RotateFrom { FirstRotation, CurrentRotation, Rotation, DeltaRotation, OtherTransformRotation }
    public enum RotateTo { FirstRotation, CurrentRotation, Rotation, DeltaRotation, OtherTransformRotation }
    public enum RotateSpace { Local, World }
}
