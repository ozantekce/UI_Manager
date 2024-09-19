namespace UIManager
{
    /// <summary>
    /// A command for opening a UI element.
    /// </summary>
    internal class OpenElementCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the OpenElementCommand class.
        /// </summary>
        /// <param name="element">The UIElement to be opened.</param>
        /// <param name="delay">The delay before executing the command.</param>
        /// <param name="animIndex">The animation index for opening the element.</param>
        public OpenElementCommand(UIElement element, float delay, int animIndex)
            : base(element, delay, animIndex)
        {
        }

        /// <summary>
        /// Executes the open element command.
        /// </summary>
        protected override void ExecuteCommand()
        {
            if (IsTerminated)
            {
                return;
            }
            _element.Open(0, AnimIndex);
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
            get => base.IsTerminated || (_element != null && _element.Status == ElementStatus.Opened);
            protected set => base.IsTerminated = value;
        }
    }
}
