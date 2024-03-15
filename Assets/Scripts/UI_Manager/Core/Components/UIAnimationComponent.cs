using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace UIManager
{

    public class UIAnimationComponent : MonoBehaviour
    {


        private Dictionary<AnimationExecuteTime, List<UIAnimationWrapper>> _dictionary;

        [SerializeField] private List<UIAnimationWrapper> _idleAnimations;
        [SerializeField] private List<UIAnimationWrapper> _openAnimations;
        [SerializeField] private List<UIAnimationWrapper> _closeAnimations;
        /*
        [SerializeField] private List<UIAnimationWrapper> _focusAnimations;
        [SerializeField] private List<UIAnimationWrapper> _pointerEnterAnimations;
        [SerializeField] private List<UIAnimationWrapper> _pointerExitAnimations;
        */

        private AnimationExecuteTime? _currentAnimationExecuteTime;
        private int? _currentAnimationIndex;


        private BaseUIAnimation _customAnimation;

        public IEnumerator PlayAnimation(UIElement element ,AnimationExecuteTime executeTime, int index = 0)
        {

            KillActiveAnimation();

            yield return new WaitUntil(() => !IsPlayingAnimation());

            BaseUIAnimation animation = GetAnimation(executeTime, index);
            if(animation == null)
            {
                _currentAnimationExecuteTime = null;
                _currentAnimationIndex = null;
                yield break;
            }

            _currentAnimationExecuteTime = executeTime;
            _currentAnimationIndex = index;
            yield return animation.Enumerator(element);
            _currentAnimationExecuteTime = null;
            _currentAnimationIndex = null;

        }


        public IEnumerator PlayCustomAnimation(UIElement element, BaseUIAnimation animation)
        {

            if (IsPlayingAnimation())
            {
                if(_currentAnimationExecuteTime.Value == AnimationExecuteTime.Custom)
                {
                    KillActiveAnimation();
                }
                else
                {
                    yield break;
                }
            }

            yield return new WaitUntil(() => !IsPlayingAnimation());

            _customAnimation = animation;

            _currentAnimationExecuteTime = AnimationExecuteTime.Custom;
            _currentAnimationIndex = null;
            yield return animation.Enumerator(element);
            _currentAnimationExecuteTime = null;
            _currentAnimationIndex = null;
        }



        public void KillActiveAnimation()
        {
            if (IsPlayingAnimation())
            {
                if(_currentAnimationExecuteTime.Value == AnimationExecuteTime.Custom)
                {
                    _customAnimation.Kill();
                }
                else
                {
                    BaseUIAnimation animation = GetAnimation(_currentAnimationExecuteTime.Value, _currentAnimationIndex.Value);
                    animation.Kill();
                }
            }
        }


        public bool IsPlayingAnimation()
        {
            return _currentAnimationExecuteTime.HasValue;
        }


        public Dictionary<AnimationExecuteTime, List<UIAnimationWrapper>> GetAnimations()
        {
            _dictionary ??= new Dictionary<AnimationExecuteTime, List<UIAnimationWrapper>>();

            // Check and add Idle Animations
            if (IdleAnimations != null && IdleAnimations.Count > 0)
                _dictionary[AnimationExecuteTime.Idle] = IdleAnimations;
            else
                _dictionary.Remove(AnimationExecuteTime.Idle);

            // Check and add Open Animations
            if (OpenAnimations != null && OpenAnimations.Count > 0)
                _dictionary[AnimationExecuteTime.Open] = OpenAnimations;
            else
                _dictionary.Remove(AnimationExecuteTime.Open);

            // Check and add Close Animations
            if (CloseAnimations != null && CloseAnimations.Count > 0)
                _dictionary[AnimationExecuteTime.Close] = CloseAnimations;
            else
                _dictionary.Remove(AnimationExecuteTime.Close);
            /*
            // Check and add Focus Animations
            if (FocusAnimations != null && FocusAnimations.Count > 0)
                _dictionary[AnimationExecuteTime.Focus] = FocusAnimations;
            else
                _dictionary.Remove(AnimationExecuteTime.Focus);

            // Check and add PointerEnter Animations
            if (PointerEnterAnimations != null && PointerEnterAnimations.Count > 0)
                _dictionary[AnimationExecuteTime.PointerEnter] = PointerEnterAnimations;
            else
                _dictionary.Remove(AnimationExecuteTime.PointerEnter);

            // Check and add PointerExit Animations
            if (PointerExitAnimations != null && PointerExitAnimations.Count > 0)
                _dictionary[AnimationExecuteTime.PointerExit] = PointerExitAnimations;
            else
                _dictionary.Remove(AnimationExecuteTime.PointerExit);
            */
            return _dictionary;
        }


        public void SetAnimation(AnimationExecuteTime executeTime, UIAnimationWrapper animation, int index = 0)
        {
            if(executeTime == AnimationExecuteTime.Idle) IdleAnimations[index] = animation;
            else if(executeTime == AnimationExecuteTime.Open) OpenAnimations[index] = animation;
            else if(executeTime == AnimationExecuteTime.Close) CloseAnimations[index] = animation;
            //else if(executeTime == AnimationExecuteTime.Focus) FocusAnimations[index] = animation;
            //else if(executeTime == AnimationExecuteTime.PointerEnter) PointerEnterAnimations[index] = animation;
            //else if(executeTime == AnimationExecuteTime.PointerExit) PointerExitAnimations[index] = animation;
        }


        private BaseUIAnimation GetAnimation(AnimationExecuteTime executeTime, int index = 0)
        {
            Dictionary <AnimationExecuteTime, List <UIAnimationWrapper>> dic = GetAnimations();
            if (!dic.ContainsKey(executeTime)) return null;
            else if (dic[executeTime].Count >= index) return dic[executeTime][0].Animation;
            return dic[executeTime][index].Animation;
        }


        
        private List<UIAnimationWrapper> IdleAnimations { get => _idleAnimations; set => _idleAnimations = value; }
        private List<UIAnimationWrapper> OpenAnimations { get => _openAnimations; set => _openAnimations = value; }
        private List<UIAnimationWrapper> CloseAnimations { get => _closeAnimations; set => _closeAnimations = value; }

        /*
        private List<UIAnimationWrapper> FocusAnimations { get => _focusAnimations; set => _focusAnimations = value; }
        private List<UIAnimationWrapper> PointerEnterAnimations { get => _pointerEnterAnimations; set => _pointerEnterAnimations = value; }
        private List<UIAnimationWrapper> PointerExitAnimations { get => _pointerExitAnimations; set => _pointerExitAnimations = value; }
        */

    }


}