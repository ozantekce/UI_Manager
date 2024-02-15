using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace UI_Manager
{

    public class AnimationComponent : MonoBehaviour
    {


        private Dictionary<AnimationExecuteTime, List<UiAnimationWrapper>> _dictionary;

        [SerializeField] private List<UiAnimationWrapper> _idleAnimations;
        [SerializeField] private List<UiAnimationWrapper> _openAnimations;
        [SerializeField] private List<UiAnimationWrapper> _closeAnimations;
        [SerializeField] private List<UiAnimationWrapper> _focusAnimations;
        [SerializeField] private List<UiAnimationWrapper> _pointerEnterAnimations;
        [SerializeField] private List<UiAnimationWrapper> _pointerExitAnimations;

        private AnimationExecuteTime? _currentAnimationExecuteTime;
        private int? _currentAnimationIndex;


        public IEnumerator PlayAnimation(UI_Element element ,AnimationExecuteTime executeTime, int index = 0)
        {
            if(_currentAnimationExecuteTime.HasValue)
            {
                Get(_currentAnimationExecuteTime.Value, _currentAnimationIndex.Value).Kill();
            }

            UI_Animation ui_Animation = Get(executeTime, index);
            if(ui_Animation == null)
            {
                _currentAnimationExecuteTime = null;
                _currentAnimationIndex = null;
                yield break;
            }

            _currentAnimationExecuteTime = executeTime;
            _currentAnimationIndex = index;
            yield return ui_Animation.Enumerator(element);
            _currentAnimationExecuteTime = null;
            _currentAnimationIndex = null;

        }

        public void KillActiveAnimation()
        {
            if (_currentAnimationExecuteTime.HasValue)
            {
                Get(_currentAnimationExecuteTime.Value, _currentAnimationIndex.Value).Kill();
            }
        }


        public Dictionary<AnimationExecuteTime, List<UiAnimationWrapper>> GetAnimations()
        {
            _dictionary ??= new Dictionary<AnimationExecuteTime, List<UiAnimationWrapper>>();

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

            return _dictionary;
        }


        public void SetAnimation(AnimationExecuteTime executeTime, UiAnimationWrapper animation, int index = 0)
        {
            if(executeTime == AnimationExecuteTime.Idle) IdleAnimations[index] = animation;
            else if(executeTime == AnimationExecuteTime.Open) OpenAnimations[index] = animation;
            else if(executeTime == AnimationExecuteTime.Close) CloseAnimations[index] = animation;
            else if(executeTime == AnimationExecuteTime.Focus) FocusAnimations[index] = animation;
            else if(executeTime == AnimationExecuteTime.PointerEnter) PointerEnterAnimations[index] = animation;
            else if(executeTime == AnimationExecuteTime.PointerExit) PointerExitAnimations[index] = animation;
        }


        private UI_Animation Get(AnimationExecuteTime executeTime, int index = 0)
        {
            Dictionary < AnimationExecuteTime, List <UiAnimationWrapper>> dic = GetAnimations();
            if (!dic.ContainsKey(executeTime)) return null;
            else if (dic[executeTime].Count >= index) return dic[executeTime][0].Animation;
            return dic[executeTime][index].Animation;
        }


        
        public List<UiAnimationWrapper> IdleAnimations { get => _idleAnimations; set => _idleAnimations = value; }
        private List<UiAnimationWrapper> OpenAnimations { get => _openAnimations; set => _openAnimations = value; }
        public List<UiAnimationWrapper> CloseAnimations { get => _closeAnimations; set => _closeAnimations = value; }
        public List<UiAnimationWrapper> FocusAnimations { get => _focusAnimations; set => _focusAnimations = value; }
        public List<UiAnimationWrapper> PointerEnterAnimations { get => _pointerEnterAnimations; set => _pointerEnterAnimations = value; }
        public List<UiAnimationWrapper> PointerExitAnimations { get => _pointerExitAnimations; set => _pointerExitAnimations = value; }


    }


}