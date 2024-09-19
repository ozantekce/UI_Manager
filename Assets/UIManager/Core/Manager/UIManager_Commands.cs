using System.Collections.Generic;

namespace UIManager
{
    public partial class UIManager
    {

        //Optional
        private const bool ExecuteOneCommandPerFrame = false;

        private Heap<BaseCommand> _commands = new Heap<BaseCommand>();
        private Dictionary<UIElement, HashSet<BaseCommand>> _waitingCommands = new Dictionary<UIElement, HashSet<BaseCommand>>();


        private void Update()
        {
        ExecuteNextCommand:
            if (_commands.IsEmpty() || !_commands.Peek().IsReady)
                return;

            BaseCommand command = _commands.Remove();
            HashSet<BaseCommand> waitingSet = _waitingCommands.GetValueOrDefault(command.Element, new HashSet<BaseCommand>());
            waitingSet.Remove(command);

            if (command.IsReady)
            {
                command.Execute();
                if (!ExecuteOneCommandPerFrame)
                {
                    goto ExecuteNextCommand;
                }
            }

        }


        public void LoadScene(string sceneName, float delay)
        {
            AddCommand(new LoadSceneCommand(sceneName, delay));
        }

        public void LoadScene(string sceneName)
        {
            LoadScene(sceneName, 0f);
        }


        public void SwapUIElements(UIElement openElement, int openAnimIndex, UIElement closeElement, int closeAnimIndex, float delay)
        {
            OpenElementCommand openCommand = new OpenElementCommand(openElement, 0, openAnimIndex);
            CloseElementCommand closeCommand = new CloseElementCommand(closeElement, 0, closeAnimIndex);
            AddCommand(new SwapElementsCommand(openCommand, closeCommand, delay));
        }




        public void OpenUIElement(UIElement element, float delay, int animIndex)
        {
            AddCommand(new OpenElementCommand(element, delay, animIndex));
        }

        public void OpenUIElement(UIElement element, float delay)
        {
            OpenUIElement(element, delay, 0);
        }

        public void OpenUIElement(UIElement element, int animIndex)
        {
            OpenUIElement(element, 0f, animIndex);
        }

        public void OpenUIElement(UIElement element)
        {
            OpenUIElement(element, 0f, 0);
        }





        public void CloseUIElement(UIElement element, float delay, int animIndex)
        {
            AddCommand(new CloseElementCommand(element, delay, animIndex));
        }
        public void CloseUIElement(UIElement element, float delay)
        {
            CloseUIElement(element, delay, 0);
        }

        public void CloseUIElement(UIElement element, int animIndex)
        {
            CloseUIElement(element, 0f, animIndex);
        }

        public void CloseUIElement(UIElement element)
        {
            CloseUIElement(element, 0f, 0);
        }



        public void ForceOpenUIElement(UIElement element)
        {
            element.ForceOpen();
        }

        public void ForceCloseUIElement(UIElement element)
        {
            element.ForceClose();
        }


        public void CloseAllUIElements(float delay = 0f, int animIndex = 0)
        {
            foreach (UIElement e in _elements)
            {
                CloseUIElement(e, delay, animIndex);
            }
        }


        public void QuitApplication(float delay = 0)
        {
            AddCommand(new QuitApplicationCommand(delay));
        }


        private void AddCommand(BaseCommand command)
        {
            _commands.Insert(command);
            HashSet<BaseCommand> waitingSet = _waitingCommands.GetValueOrDefault(command.Element, new HashSet<BaseCommand>());
            waitingSet.Add(command);
            _waitingCommands[command.Element] = waitingSet;
        }


        public void SkipCommands(UIElement element)
        {
            HashSet<BaseCommand> waitingSet = _waitingCommands.GetValueOrDefault(element, new HashSet<BaseCommand>());

            foreach (var command in waitingSet)
            {
                command.SkipCommand();
            }

        }



    }
}

