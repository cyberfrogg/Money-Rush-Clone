using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Level
{
    public class LevelSwitch
    {
        private LevelSwitchConfig _config;
        private int _currentLevel = -1;

        public LevelSwitch(LevelSwitchConfig config)
        {
            _config = config;
        }

        public void LoadNextLevel()
        {
            _currentLevel++;
            SceneManager.LoadScene(_config.GetLevel(_currentLevel));
        }
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}