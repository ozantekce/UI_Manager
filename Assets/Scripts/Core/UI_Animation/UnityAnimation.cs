using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI_Manager
{

    [System.Serializable]
    public class UnityAnimation : UI_Animation
    {

        public override UI_AnimationType AnimationType => UI_AnimationType.Unity;

        [SerializeField]
        private AnimationClip _clip;


        private Animation _animation;

        private WaitForEndOfFrame _waitForEndOfFrame;

        

        public override IEnumerator Enumerator(UI_Element element)
        {

            if (_clip == null) yield break;

            _clip.legacy = true;

            _animation = element.gameObject.GetComponent<Animation>();
            if (_animation == null) _animation = element.gameObject.AddComponent<Animation>();

            if (_animation.GetClip(_clip.name) == null)
            {
                _animation.AddClip(_clip, _clip.name);
            }
            element.gameObject.SetActive(true);
            _animation.Play(_clip.name);

            yield return _waitForEndOfFrame;

            _animation.Play(_clip.name);
            while (_animation.IsPlaying(_clip.name))
            {
                yield return _waitForEndOfFrame;
            }

        }

        public override void Kill()
        {
            _animation.Stop();
        }
    }
}

