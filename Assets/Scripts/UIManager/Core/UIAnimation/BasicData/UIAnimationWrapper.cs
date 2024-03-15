using UnityEngine;

namespace UIManager
{


    [System.Serializable]
    public class UIAnimationWrapper
    {
        public UIAnimationType animType;

        [SerializeField, ShowInEnum(nameof(animType), nameof(UIAnimationType.Tween))] private TweenAnimation _tweenAnimation = new TweenAnimation();
        [SerializeField, ShowInEnum(nameof(animType), nameof(UIAnimationType.Unity))] private UnityAnimation _unityAnimation = new UnityAnimation();



        public BaseUIAnimation Animation
        {
            get
            {
                return animType switch
                {
                    UIAnimationType.Tween => _tweenAnimation,
                    UIAnimationType.Unity => _unityAnimation,
                    _ => null,
                };
            }
        }
    }


}