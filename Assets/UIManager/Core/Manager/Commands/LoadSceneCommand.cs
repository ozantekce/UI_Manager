using UnityEngine.SceneManagement;

namespace UIManager
{
    internal class LoadSceneCommand : BaseCommand
    {
        private string _sceneName;

        public LoadSceneCommand(string sceneName, float delay) : base(delay)
        {
            this._sceneName = sceneName;
        }

        protected override void ExecuteCommand()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }

}