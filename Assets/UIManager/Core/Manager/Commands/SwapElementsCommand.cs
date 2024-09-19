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
            if (IsTerminated)
            {
                return;
            }
            _openCommand.Execute();
            _closeCommand.Execute();
        }


        public override void OnManagerDestroy()
        {
            UIElement element1 = _openCommand.Element;
            UIElement element2 = _closeCommand.Element;
            if(element1 != null)
                element1.ForceClose();
            if(element2 != null)
                element2.ForceClose();
        }


        public override bool IsTerminated
        {
            get => base.IsTerminated || _openCommand == null || _closeCommand == null ||
                (_openCommand.Element != null && _openCommand.Element.Status == ElementStatus.Opened) ||
                (_closeCommand.Element != null && _closeCommand.Element.Status == ElementStatus.Closed);
            protected set => base.IsTerminated = value;
        }
    }
}
