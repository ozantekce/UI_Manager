using UnityEngine;

namespace UIManager
{
    internal class QuitApplicationCommand : BaseCommand
    {
        public QuitApplicationCommand(float delay) : base(delay) { }

        protected override void ExecuteCommand()
        {
            Application.Quit();
        }

    }

}
