using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_Manager
{
    public partial class UI_Manager
    {

        private abstract class Command : IComparable<Command>
        {
            protected UI_Element _element;
            private float _delay;
            private bool _waitUntilTerminated;
            private int _animIndex;

            private bool _isTerminated;
            private float _createTime;
            public Command(UI_Element element, float delay, int animIndex)
            {
                this._element = element;
                this._createTime = Time.time;
                this._delay = delay;
                this._animIndex = animIndex;
            }

            public Command(float delay = 0)
            {
                _createTime = Time.time;
                this._delay = delay;
            }

            public Command()
            {
                _createTime = Time.time;
            }

            public void Execute()
            {
                Execute_();
            }
            protected abstract void Execute_();

            #region GetterSetter
            public float RemaniderTime { get { return Delay - (Time.time - _createTime); } }
            public float Delay { get => _delay; set => _delay = value; }
            public bool IsReady { get { return Time.time - (_createTime) >= Delay; } }
            public virtual bool IsTerminated { get => _isTerminated; protected set => _isTerminated = value; }
            public bool WaitUntilTerminated { get => _waitUntilTerminated; set => _waitUntilTerminated = value; }
            public int AnimIndex { get => _animIndex; set => _animIndex = value; }
            #endregion
            public int CompareTo(Command other)
            {
                if (this.RemaniderTime > other.RemaniderTime) return +1;
                else if (this.RemaniderTime < other.RemaniderTime) return -1;
                else return 0;
            }


        }

        private class OpenUIElementCommand : Command
        {
            public OpenUIElementCommand(UI_Element element, float delay, int animIndex) : base(element, delay, animIndex)
            {
            }

            protected override void Execute_()
            {
                if (_element == null || _element.Status != UIElementStatus.Closed)
                {
                    IsTerminated = true;
                    return;
                }
                _element.Open(0, AnimIndex);
            }

            public override bool IsTerminated
            {
                get
                {
                    return IsTerminated || _element.Status == UIElementStatus.Opened;
                }
            }
        }

        private class CloseUIElementCommand : Command
        {
            public CloseUIElementCommand(UI_Element element, float delay, int animIndex) : base(element, delay, animIndex)
            {
            }

            protected override void Execute_()
            {
                if (_element == null || _element.Status != UIElementStatus.Opened)
                {
                    IsTerminated = true;
                    return;
                }
                _element.Close(0, AnimIndex);
            }

            public override bool IsTerminated
            {
                get
                {
                    return IsTerminated || _element.Status == UIElementStatus.Closed;
                }
            }

        }

        private class QuitAppCommand : Command
        {

            public QuitAppCommand(float delay) : base(delay)
            {
            }

            protected override void Execute_()
            {
                Application.Quit();
            }

        }

        private class LoadSceneCommand : Command
        {
            private string _sceneName;

            public LoadSceneCommand(string sceneName, float delay) : base(delay)
            {
                this._sceneName = sceneName;
            }

            protected override void Execute_()
            {
                SceneManager.LoadScene(_sceneName);
            }
        }

    }
}

