using System.Collections;


namespace UIManager
{

    [System.Serializable]
    public abstract class BaseUIAnimation
    {

        public abstract UIAnimationType AnimationType { get; }
        public abstract bool IsPlaying { get; }
        public abstract IEnumerator Enumerator(UIElement element);

        public abstract void Kill();

        public static BaseUIAnimation Create(UIAnimationType animationType)
        {
            if(animationType == UIAnimationType.Tween) return new TweenAnimation();
            else if(animationType == UIAnimationType.Unity) return new UnityAnimation();
            else return null;
        }

    }


}
