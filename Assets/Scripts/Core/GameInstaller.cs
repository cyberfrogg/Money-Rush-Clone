using Core.Enviroment.FinishScoreCounting;
using Core.Level;
using Core.PlayerMoneyWad;
using Core.UI.ScreenSystem;
using UnityEngine;

namespace Core
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerInstaller _playerInstaller;
        [SerializeField] private ScreensInstaller _screensInstaller;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private LevelSwitchConfig _levelSwitchConfig;

        private Game _game;
        private LevelSwitch _levelSwitch;

        private void Awake()
        {
            _levelSwitch = new LevelSwitch(_levelSwitchConfig);
            _game = new Game(_playerInstaller.Create(), _screensInstaller.CreateScreens(), _scoreCounter, _levelSwitch);
        }
        private void OnDestroy()
        {
            _game.Stop();
        }
    }
}
