using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Level
{
    public class LevelSwitch
    {
        private LevelSwitchConfig _config;
        private int _currentLevel
        {
            get
            {
                return PlayerPrefs.GetInt("level");
            }
            set
            {
                PlayerPrefs.SetInt("level", value);
            }
        }

        public LevelSwitch(LevelSwitchConfig config)
        {
            _config = config;

            if (!PlayerPrefs.HasKey("level"))
            {
                PlayerPrefs.SetInt("level", 0);
            }
        }

        public void LoadNextLevel()
        {
            _currentLevel++;

            if (_currentLevel >= _config.Levels.Count)
            {
                _currentLevel = 0;
            }

            SceneManager.LoadScene(_config.GetLevel(_currentLevel));
        }
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}