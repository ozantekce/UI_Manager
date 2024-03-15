using System.Collections;
using UnityEngine;

namespace UIManager
{

    [System.Serializable]
    public class UnityAnimation : BaseUIAnimation
    {

        public override UIAnimationType AnimationType => UIAnimationType.Unity;

        

        [SerializeField] private AnimationClip _clip;

        private Animation _animation;
        private WaitForEndOfFrame _waitForEndOfFrame;

        public override IEnumerator Enumerator(UIElement element)
        {

            if (_clip == null) yield break;

            _waitForEndOfFrame ??= new WaitForEndOfFrame();

            _clip.legacy = true;

            if (!element.gameObject.TryGetComponent(out _animation))
            {
                _animation = element.gameObject.AddComponent<Animation>();
            }

            if (_animation.GetClip(_clip.name) == null)
            {
                _animation.AddClip(_clip, _clip.name);
            }

            element.gameObject.SetActive(true);
            _animation.Play(_clip.name);

            yield return _waitForEndOfFrame;

            _animation.Play(_clip.name);
            while (IsPlaying)
            {
                yield return _waitForEndOfFrame;
            }

        }

        public override void Kill()
        {
            _animation.Stop();
        }


        public override bool IsPlaying => _animation != null && _clip.name != null && _animation.IsPlaying(_clip.name);

    }
}

