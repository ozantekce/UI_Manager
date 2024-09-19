namespace UIManager
{
    /// <summary>
    /// A command for closing a UI element.
    /// </summary>
    internal class CloseElementCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the CloseElementCommand class.
        /// </summary>
        /// <param name="element">The UIElement to be closed.</param>
        /// <param name="delay">The delay before executing the command.</param>
        /// <param name="animIndex">The animation index for closing the element.</param>
        public CloseElementCommand(UIElement element, float delay, int animIndex)
            : base(element, delay, animIndex)
        {
        }

        /// <summary>
        /// Executes the close element command.
        /// </summary>
        protected override void ExecuteCommand()
        {
            if (IsTerminated)
            {
                return;
            }
            _element.Close(0, AnimIndex);
        }

        public override void OnManagerDestroy()
        {
            if (Element != null)
                Element.ForceClose();
        }



        /// <summary>
        /// Gets a value indicating whether the command is terminated.
        /// </summary>
        public override bool IsTerminated
        {
            get => base.IsTerminated || (_element != null && _element.Status == UIElementStatus.Closed);
            protected set => base.IsTerminated = value;
        }
    }
}
