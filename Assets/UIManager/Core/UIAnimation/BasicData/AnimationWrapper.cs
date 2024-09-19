using EditorTools;
using UnityEngine;

namespace UIManager
{


    [System.Serializable]
    public class AnimationWrapper
    {
        [field: SerializeField] public UIAnimationType AnimType { get; private set; }

        [SerializeField, ShowInEnum(nameof(AnimType), nameof(UIAnimationType.Tween))] private TweenAnimation TweenAnimation = new TweenAnimation();
        [SerializeField, ShowInEnum(nameof(AnimType), nameof(UIAnimationType.Unity))] private UnityAnimation UnityAnimation = new UnityAnimation();



        public BaseUIAnimation Animation
        {
            get
            {
                return AnimType switch
                {
                    UIAnimationType.Tween => TweenAnimation,
                    UIAnimationType.Unity => UnityAnimation,
                    _ => null,
                };
            }
        }
    }


}