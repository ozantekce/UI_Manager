using UnityEngine;

namespace UIManager
{

    internal class SwapElementsCommand : BaseCommand
    {

        private OpenElementCommand _openCommand;
        private CloseElementCommand _closeCommand;

        public SwapElementsCommand(OpenElementCommand openCommand, CloseElementCommand closeCommand, float delay) : base(delay)
        {
            _openCommand = openCommand;
            _closeCommand = closeCommand;
        }

        protected override void ExecuteCommand()
        {
            Debug.Log("Swap - 1");
            if (IsTerminated)
            {
                return;
            }
            Debug.Log("Swap - 2");
            _openCommand.Execute();
            _closeCommand.Execute();
        }

        public override bool IsTerminated
        {
            get => base.IsTerminated || _openCommand == null || _closeCommand == null ||
                (_openCommand.Element != null && _openCommand.Element.Status == UIElementStatus.Opened) ||
                (_closeCommand.Element != null && _closeCommand.Element.Status == UIElementStatus.Closed);
            protected set => base.IsTerminated = value;
        }
    }
}
