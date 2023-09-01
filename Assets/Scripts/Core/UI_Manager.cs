using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_Manager : MonoBehaviour
{

    private static UI_Manager s_Instance;


    [SerializeField, InterfaceType(typeof(UI_Element))]
    private MonoBehaviour[] _uiElements;


    private Dictionary<UIElementType, List<UI_Element>> _typeElements;


    private Heap<Command> _commands;




    private void Awake()
    {
        if (s_Instance != null)
        {
            DestroyImmediate(this);
            return;
        }
        MakeSingleton();

        _typeElements = new Dictionary<UIElementType, List<UI_Element>>();
        _commands = new Heap<Command>();

        FindAllUIElements();

        foreach (MonoBehaviour uiElement in _uiElements)
        {
            UI_Element ie = uiElement as UI_Element;
            List<UI_Element> temp = _typeElements.GetValueOrDefault(ie.ElementType, new List<UI_Element>());
            temp.Add(ie);
            _typeElements[ie.ElementType] = temp;

            ie.ConfigurationsAwake();
        }




    }


    private void Start()
    {
        foreach (MonoBehaviour uiElement in _uiElements)
        {
            UI_Element ie = uiElement as UI_Element;
            ie.ConfigurationsStart();
        }

    }


    //Optional
    private const bool ExecuteOneCommandPerFrame = false;

    private void Update()
    {
    ExecuteNextCommand:
        if (_commands.IsEmpty() || !_commands.Peek().IsReady)
            return;

        Command command = _commands.Remove();

        if (command.IsReady)
        {
            command.Execute();
            if (!ExecuteOneCommandPerFrame)
            {
                goto ExecuteNextCommand;
            }
        }

    }


    public void FindAllUIElements()
    {

        Scene currentScene = SceneManager.GetActiveScene();

        List<MonoBehaviour> allEntities = new List<MonoBehaviour>();
        foreach (GameObject rootGameObject in currentScene.GetRootGameObjects())
        {
            MonoBehaviour[] subs = rootGameObject.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (MonoBehaviour entity in subs)
            {
                if (entity is UI_Element)
                {
                    allEntities.Add(entity);
                }
            }
        }

        _uiElements = allEntities.ToArray();

    }

    public void LoadScene(string sceneName)
    {
        LoadScene(sceneName, 0);
    }
    public void LoadScene(string sceneName, float delay = 0)
    {
        AddCommand(new LoadSceneCommand(sceneName, delay));
    }

    public void OpenUIElement(UI_Element element) { OpenUIElement(element, 0); }
    public void OpenUIElement(UI_Element element, float delay = 0){ AddCommand(new OpenUIElementCommand(element, delay)); }

    public void CloseUIElement(UI_Element element) { CloseUIElement(element, 0f); }
    public void CloseUIElement(UI_Element element, float delay) { AddCommand(new CloseUIElementCommand(element, delay)); }

    public void ForceOpenUIElement(UI_Element element)
    {
        element.Status = UIElementStatus.Opened;
        element.gameObject.SetActive(true);
    }

    public void ForceCloseUIElement(UI_Element element)
    {
        element.Status = UIElementStatus.Closed;
        element.gameObject.SetActive(true);
    }

    public void CloseAllUIElements(UIElementType type,float delay = 0)
    {
        if(_typeElements.TryGetValue(type, out List<UI_Element> elements))
        {
            foreach (UI_Element e in elements)
            {
                CloseUIElement(e,delay);
            }
        }
    }


    public void QuitApplication(float delay = 0)
    {
        AddCommand(new QuitAppCommand(delay));
    }



    private void AddCommand(Command command, params Command[] subs)
    {
        command.AddSubCommands(subs);
        _commands.Insert(command);
    }



    #region GetterSetter

    public bool TryGetElements(UIElementType elementType, out List<UI_Element> elements)
    {
        if (_typeElements.TryGetValue(elementType, out List<UI_Element> list))
        {
            elements = list;
            return true;
        }
        elements = null;
        return false;
    }

    #endregion


    private void MakeSingleton() { s_Instance = this; }

    public static UI_Manager Instance { get => s_Instance; }




    #region Commands
    private abstract class Command : IComparable<Command>
    {

        protected UI_Element _element;
        private float _delay;
        private bool _waitUntilTerminated;

        private bool _isTerminated;
        private float _createTime;
        private List<Command> _subCommands = new List<Command>();
        public Command(UI_Element element, float delay = 0)
        {
            this._element = element;
            this._createTime = Time.time;
            this._delay = delay;
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

        public void AddSubCommands(params Command[] subs) { _subCommands.AddRange(subs); }

        public void Execute()
        {
            for (int i = 0; i < _subCommands.Count; i++) _subCommands[i].Execute();
            Execute_();
        }
        protected abstract void Execute_();
        #region GetterSetter
        public float RemaniderTime { get { return Delay - (Time.time - _createTime); } }
        public float Delay { get => _delay; set => _delay = value; }
        public bool IsReady { get { return Time.time - (_createTime) >= Delay; } }
        public virtual bool IsTerminated { get => _isTerminated; protected set => _isTerminated = value; }
        public bool WaitUntilTerminated { get => _waitUntilTerminated; set => _waitUntilTerminated = value; }
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
        public OpenUIElementCommand(UI_Element element, float delay = 0) : base(element, delay)
        {
        }

        protected override void Execute_()
        {
            if (_element == null || _element.Status != UIElementStatus.Closed)
            {
                IsTerminated = true;
                return;
            }
            _element.Open();
        }

        public override bool IsTerminated
        {
            get
            {
                return IsTerminated
                    || _element.Status == UIElementStatus.Opened;
            }
        }
    }

    private class CloseUIElementCommand : Command
    {
        public CloseUIElementCommand(UI_Element element, float delay = 0) : base(element, delay)
        {
        }

        protected override void Execute_()
        {
            if (_element == null || _element.Status != UIElementStatus.Opened)
            {
                IsTerminated = true;
                return;
            }
            _element.Close();
        }

        public override bool IsTerminated
        {
            get
            {
                return IsTerminated
                    || _element.Status == UIElementStatus.Closed;
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


    #endregion


}
public static class UI_ManagerExtensions
{

    public static void OpenUIElement(this string alias, float delay = 0)
    {
        UI_Element element = alias.GetEntity<UI_Element>();
        UI_Manager.Instance.OpenUIElement(element, delay);
    }

    public static void OpenUIElement(this UI_Element element, float delay = 0)
    {
        UI_Manager.Instance.OpenUIElement(element, delay);
    }

    public static void CloseUIElement(this string alias, float delay = 0)
    {
        UI_Element element = alias.GetEntity<UI_Element>();
        UI_Manager.Instance.CloseUIElement(element, delay);
    }

    public static void CloseUIElement(this UI_Element element, float delay = 0)
    {
        UI_Manager.Instance.CloseUIElement(element, delay);
    }

    public static void LoadScene(this string sceneName, float delay = 0)
    {
        UI_Manager.Instance.LoadScene(sceneName, delay);
    }

    
    public static void SetExtendedText(this string alias, string text)
    {
        ExtendedText et = alias.GetEntity<ExtendedText>();
        et.Text = text;
    }

    public static void SetExtendedTextMethod(this string alias, TextMethod textMethod)
    {
        ExtendedText et = alias.GetEntity<ExtendedText>();
        et.TextMethod = textMethod;
    }

}

