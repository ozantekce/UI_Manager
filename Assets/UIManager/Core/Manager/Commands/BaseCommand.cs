using System;
using UnityEngine;

namespace UIManager
{
    /// <summary>
    /// Represents a base command for UI elements.
    /// </summary>
    internal abstract class BaseCommand : IComparable<BaseCommand>
    {
        protected UIElement _element;
        private float _delay;
        private bool _waitUntilTerminated;
        private int _animIndex;

        private bool _isTerminated;
        private bool _skipCommand;
        private readonly float _createTime;

        protected BaseCommand(UIElement element, float delay, int animIndex)
        {
            _element = element;
            _createTime = Time.time;
            _delay = delay;
            _animIndex = animIndex;
        }

        protected BaseCommand(float delay = 0)
        {
            _createTime = Time.time;
            _delay = delay;
        }

        protected BaseCommand()
        {
            _createTime = Time.time;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public void Execute()
        {
            if(!_skipCommand)
                ExecuteCommand();
            else
                _isTerminated = true;
        }

        /// <summary>
        /// The implementation of the execution command.
        /// </summary>
        protected abstract void ExecuteCommand();


        public virtual void OnManagerDestroy()
        {

        }

        public virtual void SkipCommand()
        {
            _skipCommand = true;
        }


        public float RemainingTime => Delay - (Time.time - _createTime);

        public float Delay { get => _delay; private set => _delay = value; }

        public bool IsReady => Time.time - _createTime >= Delay || _skipCommand;

        public virtual bool IsTerminated { get => _isTerminated; protected set => _isTerminated = value; }

        public bool WaitUntilTerminated { get => _waitUntilTerminated; private set => _waitUntilTerminated = value; }

        public int AnimIndex { get => _animIndex; private set => _animIndex = value; }
        public UIElement Element => _element;

        public int CompareTo(BaseCommand other)
        {
            return RemainingTime.CompareTo(other.RemainingTime);
        }
    }
}
