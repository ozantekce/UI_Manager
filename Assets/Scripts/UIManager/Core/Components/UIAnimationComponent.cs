using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{
    /// <summary>
    /// Manages animations for UI elements, including playing, killing, and tracking animations based on their execution time.
    /// </summary>
    public class UIAnimationComponent : MonoBehaviour
    {
        /// <summary>
        /// Stores animations grouped by their execution times.
        /// </summary>
        private Dictionary<AnimationExecuteTime, List<UIAnimationWrapper>> _dictionary;

        /// <summary>
        /// Serialized list of idle animations available for UI elements.
        /// </summary>
        [field: SerializeField] private List<UIAnimationWrapper> IdleAnimations { get; set; }

        /// <summary>
        /// Serialized list of animations for opening UI elements.
        /// </summary>
        [field: SerializeField] private List<UIAnimationWrapper> OpenAnimations { get; set; }

        /// <summary>
        /// Serialized list of animations for closing UI elements.
        /// </summary>
        [field: SerializeField] private List<UIAnimationWrapper> CloseAnimations { get; set; }

        /// <summary>
        /// Tracks the type of the currently playing animation.
        /// </summary>
        private AnimationExecuteTime? _currentAnimationExecuteTime;

        /// <summary>
        /// Tracks the index of the currently playing animation within its type group.
        /// </summary>
        private int? _currentAnimationIndex;

        /// <summary>
        /// Tracks any custom animation currently being played.
        /// </summary>
        private BaseUIAnimation _customAnimation;

        /// <summary>
        /// Plays a specified animation based on execution time and index.
        /// </summary>
        /// <param name="element">The UI element to animate.</param>
        /// <param name="executeTime">The time at which the animation should be executed.</param>
        /// <param name="index">The index of the animation in the list (defaults to 0).</param>
        public IEnumerator PlayAnimation(UIElement element, AnimationExecuteTime executeTime, int index = 0)
        {
            KillActiveAnimation();

            yield return new WaitUntil(() => !IsPlayingAnimation());

            BaseUIAnimation animation = GetAnimation(executeTime, index);
            if (animation == null)
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

        /// <summary>
        /// Plays a custom animation provided by the user.
        /// </summary>
        /// <param name="element">The UI element to animate.</param>
        /// <param name="animation">The custom animation to play.</param>
        public IEnumerator PlayCustomAnimation(UIElement element, BaseUIAnimation animation)
        {
            if (IsPlayingAnimation())
            {
                if (_currentAnimationExecuteTime.Value == AnimationExecuteTime.Custom)
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

        /// <summary>
        /// Immediately stops any active animation.
        /// </summary>
        public void KillActiveAnimation()
        {
            if (IsPlayingAnimation())
            {
                if (_currentAnimationExecuteTime.Value == AnimationExecuteTime.Custom)
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

        /// <summary>
        /// Determines if there is an animation currently playing.
        /// </summary>
        /// <returns>True if an animation is playing, otherwise false.</returns>
        public bool IsPlayingAnimation()
        {
            return _currentAnimationExecuteTime.HasValue;
        }

        /// <summary>
        /// Retrieves all animations categorized by their execution time.
        /// </summary>
        /// <returns>A dictionary of animations grouped by execution time.</returns>
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

            return _dictionary;
        }

        /// <summary>
        /// Sets a specific animation for a given execution time and index.
        /// </summary>
        /// <param name="executeTime">The execution time category of the animation.</param>
        /// <param name="animation">The animation wrapper to set.</param>
        /// <param name="index">The index at which to place the animation in its list.</param>
        public void SetAnimation(AnimationExecuteTime executeTime, UIAnimationWrapper animation, int index = 0)
        {
            if (executeTime == AnimationExecuteTime.Idle) IdleAnimations[index] = animation;
            else if (executeTime == AnimationExecuteTime.Open) OpenAnimations[index] = animation;
            else if (executeTime == AnimationExecuteTime.Close) CloseAnimations[index] = animation;
        }

        /// <summary>
        /// Retrieves a specific animation from the dictionary based on execution time and index.
        /// </summary>
        /// <param name="executeTime">The execution time for the animation.</param>
        /// <param name="index">The index of the animation in its list.</param>
        /// <returns>The specified UI animation if it exists; otherwise, null.</returns>
        private BaseUIAnimation GetAnimation(AnimationExecuteTime executeTime, int index = 0)
        {
            Dictionary<AnimationExecuteTime, List<UIAnimationWrapper>> dic = GetAnimations();
            if (!dic.ContainsKey(executeTime)) return null;
            else if (dic[executeTime].Count <= index) return null;
            return dic[executeTime][index].Animation;
        }
    }
}
