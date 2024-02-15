using DG.Tweening;
using UnityEngine;

namespace UI_Manager
{
    [System.Serializable]
    public class RotateTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Rotate;
        public enum RotateFrom { FirstRotation, CurrentRotation, Rotation, DeltaRotation, OtherTransformRotation }
        public enum RotateTo { FirstRotation, CurrentRotation, Rotation, DeltaRotation, OtherTransformRotation }
        public enum Space { Local, World }

        public Space space;
        public RotateFrom rotateFrom;
        public RotateTo rotateTo;

        [ShowInEnum(nameof(rotateFrom), nameof(RotateFrom.Rotation))]
        public Vector3 startRotation;

        [ShowInEnum(nameof(rotateTo), nameof(RotateTo.Rotation))]
        public Vector3 endRotation;

        [ShowInEnum(nameof(rotateFrom), nameof(RotateFrom.OtherTransformRotation))]
        public Transform startTransform;

        [ShowInEnum(nameof(rotateTo), nameof(RotateTo.OtherTransformRotation))]
        public Transform endTransform;

        [ShowInEnum(nameof(rotateFrom), nameof(RotateFrom.DeltaRotation))]
        public Vector3 startDeltaRotation;

        [ShowInEnum(nameof(rotateTo), nameof(RotateTo.DeltaRotation))]
        public Vector3 endDeltaRotation;

        public override Tween CreateTweenImplementation(UI_Element element)
        {
            Tween tween = null;
            Quaternion rotStart = Quaternion.identity;
            Quaternion rotEnd = Quaternion.identity;

            // Handle start rotation
            switch (rotateFrom)
            {
                case RotateFrom.FirstRotation:
                    rotStart = space == Space.Local ? element.ElementInfo.Value.firstLocalRot : element.ElementInfo.Value.firstGlobalRot;
                    break;
                case RotateFrom.CurrentRotation:
                    rotStart = space == Space.Local ? element.transform.localRotation : element.transform.rotation;
                    break;
                case RotateFrom.Rotation:
                    rotStart = Quaternion.Euler(startRotation);
                    break;
                case RotateFrom.DeltaRotation:
                    rotStart = (space == Space.Local ? element.transform.localRotation : element.transform.rotation) * Quaternion.Euler(startDeltaRotation);
                    break;
                case RotateFrom.OtherTransformRotation:
                    rotStart = startTransform.rotation;
                    break;
            }

            // Handle end rotation
            switch (rotateTo)
            {
                case RotateTo.FirstRotation:
                    rotEnd = space == Space.Local ? element.ElementInfo.Value.firstLocalRot : element.ElementInfo.Value.firstGlobalRot;
                    break;
                case RotateTo.CurrentRotation:
                    rotEnd = space == Space.Local ? element.transform.localRotation : element.transform.rotation;
                    break;
                case RotateTo.Rotation:
                    rotEnd = Quaternion.Euler(endRotation);
                    break;
                case RotateTo.DeltaRotation:
                    rotEnd = (space == Space.Local ? element.transform.localRotation : element.transform.rotation) * Quaternion.Euler(endDeltaRotation);
                    break;
                case RotateTo.OtherTransformRotation:
                    rotEnd = endTransform.rotation;
                    break;
            }

            // Create the tween
            if (space == Space.Local)
            {
                element.transform.localRotation = rotStart;
                tween = element.transform.DOLocalRotateQuaternion(rotEnd, duration).SetEase(ease).SetLoops(loopCount, loopType);
            }
            else if (space == Space.World)
            {
                element.transform.rotation = rotStart;
                tween = element.transform.DORotateQuaternion(rotEnd, duration).SetEase(ease).SetLoops(loopCount, loopType);
            }

            return tween;
        }
    }
}
