using System;
using System.Collections;
using UnityEngine;


namespace UI_Manager
{

    [System.Serializable]
    public abstract class UI_Animation
    {

        public abstract UI_AnimationType AnimationType { get; }
        public abstract IEnumerator Enumerator(UI_Element element);





        public abstract void Kill();


        public static UI_Animation Create(UI_AnimationType animationType)
        {
            if(animationType == UI_AnimationType.Tween) return new TweenAnimation();
            else if(animationType == UI_AnimationType.Unity) return new UnityAnimation();
            else return null;
        }



    }



    [System.Serializable]
    public class UiAnimationWrapper : ISerializationCallbackReceiver
    {
        public UI_AnimationType animType;

        [SerializeReference] private UI_Animation _animation = new TweenAnimation();

        

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {
            if (_animation == null || _animation.AnimationType != animType)
            {
                _animation = UI_Animation.Create(animType);
            }
        }

        public UI_Animation Animation { get => _animation; set => _animation = value; }
    }




    public enum UI_AnimationType
    {
        Tween = 0,
        Unity = 1,
    }




    public enum AnimationExecuteTime
    { 
        Idle = 0,
        Open = 1,
        Close = 2,
        Focus = 3,
        PointerEnter = 4,
        PointerExit = 5,
        PointerDown = 6,
        PointerUp = 7,
        PointerClick = 8,
        Drag = 9,
    }

}
