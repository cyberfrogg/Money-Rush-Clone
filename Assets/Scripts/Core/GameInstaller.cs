using Core.Enviroment.FinishScoreCounting;
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

        private Game _game;

        private void Awake()
        {
            _game = new Game(_playerInstaller.Create(), _screensInstaller.CreateScreens(), _scoreCounter);
        }
        private void OnDestroy()
        {
            _game.Stop();
        }
    }
}
